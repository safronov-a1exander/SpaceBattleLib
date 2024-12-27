using Google.Protobuf.Collections;
using Grpc.Net.Client;
using Npgsql;
using Outbox;

using var channel = GrpcChannel.ForAddress("http://localhost:5103");
Endpoint.EndpointClient client = new(channel);
List<string> messages = new();
string CONNECTION = "Host=localhost;Username=postgres;Password=postgres;Database=transactional_outbox";
await using NpgsqlConnection conn = new(CONNECTION);
await conn.OpenAsync();
await using (NpgsqlCommand cmd = new("SELECT message FROM outbox_messages WHERE status=false", conn))
await using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
{
    while (await reader.ReadAsync())
        messages.Add(reader.GetString(0));
}
//message "command;gameid;key:value;key:value;key:value"
foreach (string mes in messages)
{
    string command = mes.Split(';')[0];
    string gameid = mes.Split(';')[1];
    string mesArgs = mes.Replace(command + ";", "");
    mesArgs = mesArgs.Replace(gameid + ";", "");
    Console.WriteLine(mesArgs);

    Dictionary<string, string> dict = new();
    foreach (string arg in mesArgs.Split(';'))
    {
        Console.WriteLine(arg.Split(':')[0] + " " + arg.Split(':')[1]);
        dict.Add(arg.Split(':')[0], arg.Split(':')[1]);
    }
    try
    {
        MapField<string, string> props = new();
        props.Add(dict);
        CommandRequest message = new();
        message.Args.Add(props);
        CommandReply reply = client.MessageReceiver(message);
        Console.WriteLine(reply.Statuscode);
        if (reply.Statuscode == 200)
        {
            await using (var cmd = new NpgsqlCommand("UPDATE messages SET status=true WHERE \"message\"=(@p)", conn))
            {
                cmd.Parameters.AddWithValue("p", mes);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }
    catch
    {
        continue;
    }

}
