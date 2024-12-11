using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;
using Moq;
using System.Collections.Concurrent;

namespace SpaceBattle.Lib.Test;

public class EndpointGameTests
{
    [Fact]
    public void MainPositiveRoutingTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        BlockingCollection<ICommand> queue = new();
        BlockingCollection<ICommand> orderQueue = new();

        Mock<IUObject> obj = new();
        Dictionary<string, Dictionary<string, IUObject>> GamesObjects = new();
        Dictionary<string, IUObject> game1 = new();
        game1.Add("obj123", obj.Object);
        GamesObjects.Add("2.1", game1);

        ISender snd = new SenderAdapter(orderQueue);
        ISender internalSnd = new SenderAdapter(queue);

        IReceiver rec = new ReceiverAdapter(queue);
        IReceiver orderRec = new ReceiverAdapter(orderQueue);

        Dictionary<string, ISender> internalDicts = new(){{"1", internalSnd}};
        Dictionary<string, ISender> routeDict = new(){{"1", snd}};

        IRouter router = new Router(routeDict);

        Dictionary<string, object> ValueDictionary = new(){{"objid", "obj123"}, {"thread", "2"}, {"velocity", 1}};

        Assert.Empty(orderQueue);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetOrderSenderByThreadId", (object[] args) => {
            return routeDict[(string)args[0]];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInternalSenderByThreadId", (object[] args) => {
            return internalDicts[(string)args[0]];
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CurrentGameId", (object[] args) => {
                return "1";
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Order", (object[] args) => {
            ICommand cmd = (ICommand) args[0];
            ISender sender = IoC.Resolve<ISender>("GetOrderSenderByThreadId", IoC.Resolve<string>("CurrentGameId"));
            sender.Send(cmd);
            return sender;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) => {
            ICommand cmd = (ICommand) args[0];
            ISender sender = IoC.Resolve<ISender>("GetInternalSenderByThreadId", IoC.Resolve<string>("CurrentGameId"));
            sender.Send(cmd);
            return sender;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get Object by ids", (object[] args) =>
        {
            string GameID = (string) args[0];
            string ObjectID = (string) args[1];
            IUObject obj = GamesObjects[GameID][ObjectID];
            return obj;
        }).Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateMoveCommandFromMessage", (object[] args) =>
        {
            Dictionary<string, object> MessageContent = (Dictionary<string, object>) args[1];
            IUObject obj = IoC.Resolve<IUObject>("Get Object by ids", (string) args[0], MessageContent["objid"]);
            Mock<ICommand> cmd = new();
            return cmd.Object;
        }).Execute();

        IoC.Resolve<ISender>("Send Order", new ActionCommand(() => {}));
        router.Route("2.1", "MoveCommand", ValueDictionary);

        Assert.NotEmpty(orderQueue);

        orderRec.Receive().Execute();

        Assert.Empty(orderQueue);
    }

    [Fact]
    public void RoutingThrowsTest()
    {
        Mock<ISender> snd = new();

        snd.Setup(s => s.Send(It.IsAny<ICommand>())).Throws(new Exception());

        Dictionary<string, ISender> routeDict = new(){{"1", snd.Object}};

        IRouter router = new Router(routeDict);

        Dictionary<string, object> ValueDictionary1 = new(){{"objid", "obj123"}, {"thread", "2"}, {"velocity", 1}};

        Assert.False(router.Route("1", "MoveCommand", ValueDictionary1));
    }
}
