using Grpc.Core;
using Hwdtech;
using SpaceBattle.Lib.gRPCClient;

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

    public async override Task<newGameStatus> MigrateGame(gameStatus request, ServerCallContext context)
    {
        string gameId = request.GameId;
        string serializedGame = (string)new SerializeStrategy().Execute(gameId);

        Client.Call(request.NewServerId, serializedGame);

        return await Task.FromResult(new newGameStatus
        {
            GameStatus = "ok"
        });
    }

    public override Task<uploadStatus> UploadGame(serializedGameMessage request, ServerCallContext context)
    {
        string serializedGame = request.SerializedGame;
        ICommand newGameCommand = (ICommand)new DeserializeStrategy().Execute(serializedGame);
        IoC.Resolve<ICommand>("AddGame", "1", newGameCommand).Execute();
        return Task.FromResult(new uploadStatus
        {
            UploadStatus = "ok"
        });
    }
}
