using System.Threading.Tasks;
using Grpc.Net.Client;
using System;

namespace SpaceBattle.Lib.gRPCClient;

public class Client
{
    public static void Call(string ip, string message)
    {
        using var channel = GrpcChannel.ForAddress(ip);
        var client = new Endpoint.EndpointClient(channel);
        var response = client.UploadGame(new serializedGameMessage { SerializedGame = message });
    }
}
