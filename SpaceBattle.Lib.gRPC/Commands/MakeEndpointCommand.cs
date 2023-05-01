using SpaceBattle.Lib.gRPC.Services;
namespace SpaceBattle.Lib.gRPC;
public class MakeEndpointCommand : ICommand
{
    string[] EndpointArgs;
    public MakeEndpointCommand(object[] args)
    {
        this.EndpointArgs = (string[])args[0];
    }
    public void Execute()
    {
        var builder = WebApplication.CreateBuilder(EndpointArgs);
        // Add services to the container.
        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<EndpointService>();

        app.Run();
    }
}
