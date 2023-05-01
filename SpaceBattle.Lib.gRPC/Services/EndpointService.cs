using Grpc.Core;
using SpaceBattle.Lib.gRPC;

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
        return Task.FromResult(new CommandReply { Statuscode = 204 });
    }
}
