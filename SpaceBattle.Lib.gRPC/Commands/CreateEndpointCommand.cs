using SpaceBattle.Lib.gRPC.Services;
namespace SpaceBattle.Lib.gRPC;
public class CreateEndpointCommand : ICommand
{
    public CreateEndpointCommand(){}
    public void Execute()
    {
        var builder = WebApplication.CreateBuilder();
        // Add services to the container.
        builder.Services.AddGrpc();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        app.MapGrpcService<EndpointService>();

        app.Run();
    }
}
