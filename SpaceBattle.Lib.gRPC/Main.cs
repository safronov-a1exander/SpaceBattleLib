namespace SpaceBattle.Lib.gRPC;
using Grpc.Core;

class SpaceBattleGrpcApp
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton(new EndpointGameService((IRouter) ((new CreateEndpointGameStrategy()).Execute(2))));
        builder.Services.AddGrpc();
        var app = builder.Build();
        app.MapGrpcService<EndpointGameService>();
        app.Run();
    }
}
