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

    }


    [Fact]
    public void PosTest()
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
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.GetQuant", (object[] args) => (object)ts).Execute();
        var handlersdict = new Dictionary<Type, IDictionary<Type, IStrategy>>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Storage", (object[] props) => handlersdict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Exception", (object[] props) =>
        {
            var e = (Exception)props[0];
            var cmd = props[1];
            var strat = new ExceptionHandlerFindStrategy();
            try
            {
                strat.Execute(cmd, e);
            }
            catch(Exception)
            {
                throw (Exception)(e.Data["cmd"] = cmd!); // 4
            }
            return new ActionCommand(() => {});
        }
        ).Execute();

        //Arrange
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        var firstscope = IoC.Resolve<object>("Scopes.Current");
        var secondscope = IoC.Resolve<object>("Scopes.New", firstscope);
        var scopes = IoC.Resolve<ConcurrentDictionary<string, object>>("Storage.ScopeByGameID");
        ActionCommand emptyc = new(() => { });
        BlockingCollection<SpaceBattle.Lib.ICommand> externalQueue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100)        {
            emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,emptyc,
        };
        var externalReceiver = new ReceiverAdapter(externalQueue);
        scopes.TryAdd("game1", secondscope);
        var thread1 = IoC.Resolve<ServerThread>("Create Thread", "thread1", sender, receiver, externalReceiver, () => {
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", secondscope).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.ScopeByGameID", (object[] args) => scopegamedict).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.GetScopeByGameID", (object[] args) =>
            {
                var dict = IoC.Resolve<ConcurrentDictionary<string, object>>("Storage.ScopeByGameID");
                return dict[(string)args[0]];
            }
            ).Execute();
            IoC.Resolve<ICommand>("Scopes.Current.Set", firstscope).Execute();
            });
        var games = IoC.Resolve<ConcurrentDictionary<string, string>>("Storage.ThreadByGameID");
        games.TryAdd("game1", "thread1");

        //Act
        var gc = new GameMacroCommand("game1", secondscope);
        var mre = new ManualResetEvent(false);
        Stopwatch sw = new Stopwatch();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", "thread1", new ActionCommand(() => {
            sw.Start();
            gc.Execute();
            sw.Stop();
        })).Execute();
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", "thread1", new ActionCommand(() => mre.Set())).Execute();
        thread1.Execute();

        //Assert
        mre.WaitOne();
        Assert.True(firstscope == IoC.Resolve<object>("Scopes.Current"));
        Assert.True(sw.Elapsed >= IoC.Resolve<TimeSpan>("Game.GetQuant"));
    }
}
