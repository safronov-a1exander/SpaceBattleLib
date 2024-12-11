using Grpc.Core;
using System.Collections.Concurrent;
using Hwdtech;

namespace SpaceBattle.Lib.gRPC;

public class EndpointGameService : Endpoint.EndpointBase
{
    private IRouter _router;

    public EndpointGameService(IRouter router)
    {
        _router = router;
    }

    public override Task<CommandReply> MessageReceiver(CommandRequest request, ServerCallContext context)
    {
        int status = 500;

        Dictionary<string, object> data = (Dictionary<string, object>) new ProtobufMapperStrategy().Execute(request.Args);

        string gameId = request.Gid;
        string command = request.Command;

        if (_router.Route(gameId, command, data))
        {
            status = 200;
        }

        return Task.FromResult(new CommandReply
        {
            Statuscode = status
        });
    }
}
