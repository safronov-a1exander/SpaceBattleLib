namespace SpaceBattle.Lib.Test;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib.gRPC;
using SpaceBattle.Lib.gRPC.Services;
using Microsoft.Extensions.Logging;
using Grpc.Core;

public class EndpointTests
{
    static EndpointTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var threadsdict = new ConcurrentDictionary<string, ServerThread>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.ThreadByID", (object[] args) => threadsdict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.GetThreadByID", (object[] args) =>
            {
                var dict = IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Storage.ThreadByID");
                return dict[(string)args[0]];
            }
        ).Execute();
        var sendersdict = new ConcurrentDictionary<string, ISender>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.ISenderByID", (object[] args) => sendersdict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.GetISenderByID", (object[] args) =>
            {
                var dict = IoC.Resolve<ConcurrentDictionary<string, ISender>>("Storage.ISenderByID");
                return dict[(string)args[0]];
            }
        ).Execute();
        var startstrat = new CreateThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create Thread", (object[] args) => startstrat.Execute(args)).Execute();
        var sendstrat = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) => sendstrat.Execute(args)).Execute();
        var threadgamedict = new ConcurrentDictionary<string, string>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.ThreadByGameID", (object[] args) => threadgamedict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.GetThreadByGameID", (object[] args) =>
            {
                var dict = IoC.Resolve<ConcurrentDictionary<string, string>>("Storage.ThreadByGameID");
                return dict[(string)args[0]];
            }
        ).Execute();
    }

    [Fact]
    public void PosTestWithOptionals()
    {
        //Arrange
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        ActionCommand emptyc = new(() => { });
        BlockingCollection<SpaceBattle.Lib.ICommand> externalQueue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100)        {
            emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,
        };
        var externalReceiver = new ReceiverAdapter(externalQueue);
        var thread1 = IoC.Resolve<ServerThread>("Create Thread", "thread1", sender, receiver, externalReceiver);
        var games = IoC.Resolve<ConcurrentDictionary<string, string>>("Storage.ThreadByGameID");
        games.TryAdd("game1", "thread1");
        var cestrat = new CreateEndpointStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create Endpoint", (object[] args) => cestrat.Execute(args)).Execute();
        var request = new CommandRequest{Command = "take on me",  Gid = "game1"};
        var d = new Dictionary<string, string>(){{"take me on", "i'll be gone"}, {"09", "24"}};
        request.Args.Add(d);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Commands.AutoCreate.ByName", (object[] args) =>
            {
                var cmd = new Mock<SpaceBattle.Lib.ICommand>();
                Assert.Same(request.Command, args[0]);
                Assert.Equal(d.Values.ToArray(), args[1]);
                return cmd.Object;
            }
        ).Execute();
        //Act
        var endp = IoC.Resolve<SpaceBattle.Lib.ICommand>("Create Endpoint");
        var service = new EndpointService(new Mock<ILogger<EndpointService>>().Object);
        service.MessageReceiver(request, new Mock<ServerCallContext>().Object);
        Assert.False(receiver.isEmpty());
    }
}
