namespace SpaceBattle.Lib.Test;
using System.Diagnostics;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;

public class GameCommandTests
{
    static GameCommandTests()
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
        var scopegamedict = new ConcurrentDictionary<string, object>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.ScopeByGameID", (object[] args) => scopegamedict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.GetScopeByGameID", (object[] args) =>
            {
                var dict = IoC.Resolve<ConcurrentDictionary<string, object>>("Storage.ScopeByGameID");
                return dict[(string)args[0]];
            }
        ).Execute();
        var ts = new TimeSpan(0, 0, 0, 1);
        var handlersdict = new Dictionary<Type, IDictionary<Type, IStrategy>>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Storage", (object[] props) => handlersdict);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetQuant", (object[] args) => (object)ts).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Exception", (object[] props) => 
        {
            var e = (Exception)props[0];
            var cmd = props[1];
            try
            {
                var strat = new ExceptionHandlerFindStrategy().Execute(e, cmd);
            }
            catch(Exception)
            {
                throw (Exception)(e.Data["cmd"] = cmd!); // 4
            }
        }
        ).Execute();
    }
    

    [Fact]
    public void PosTest()
    {
        //Arrange
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        var thread1 = IoC.Resolve<ServerThread>("Create Thread", "thread1", sender, receiver);
        var games = IoC.Resolve<ConcurrentDictionary<string, string>>("Storage.ThreadByGameID");
        games.TryAdd("game1", "thread1");
        var scopes = IoC.Resolve<ConcurrentDictionary<string, object>>("Storage.ScopeByGameID");
        scopes.TryAdd("game1", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root")));
        var firstscope = IoC.Resolve<object>("Scopes.Current");
        //Act
        var gc = new GameCommand("game1");
        var mre = new ManualResetEvent(false);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", "thread1", gc).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", "thread1", new ActionCommand(() => mre.Set())).Execute();
        thread1.Execute();
        
        //Assert
        mre.WaitOne();
        Assert.True(gc.IsExecuted());
        Assert.True(firstscope == IoC.Resolve<object>("Scopes.Current"));
    }
}
