using Grpc.Core;
using Hwdtech;

namespace SpaceBattle.Lib.gRPC.Services;

public class EndpointService : Endpoint.EndpointBase
{
    private readonly ILogger<EndpointService> _logger;
    public EndpointService(ILogger<EndpointService> logger)
    {
        _logger = logger;
    }

    public override Task<CommandReply> MessageReceiver(CommandRequest request, ServerCallContext context)
    {
        string command = request.Command;
        string gid = request.Gid;

        var args = request.Args.Values.ToArray<string>();
        var thread = IoC.Resolve<string>("Storage.GetThreadByGameID", gid);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", thread, IoC.Resolve<SpaceBattle.Lib.ICommand>("Commands.AutoCreate.ByName", command, args)).Execute();
        return Task.FromResult(new CommandReply { Statuscode = 204 });
    }
}
