namespace SpaceBattle.Lib.Test;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

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
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Create And Start Thread", (object[] args) => startstrat.Execute(args)).Execute();
        var sendstrat = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) => sendstrat.Execute(args)).Execute();
    }

    [Fact]
    public void PosTestWithOptionals()
    {
        //Arrange
        ManualResetEvent mre = new ManualResetEvent(false);
        var cmd = () =>
            {
                mre.Set();
            };
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        var thread1 = IoC.Resolve<ServerThread>("Create And Start Thread", "thread1", sender, receiver, cmd);
        thread1.Execute();
        //Act
        
        //Assert
        Assert.True(IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Storage.ThreadByID").ContainsKey("thread1"));
        Assert.True(IoC.Resolve<ConcurrentDictionary<string, ISender>>("Storage.ISenderByID").ContainsKey("thread1"));
        Assert.False(thread1.IsReceiverEmpty());
        mre.WaitOne();
        Assert.True(thread1.IsReceiverEmpty());
    }

    [Fact]
    public void PosTestWithoutOptionals()
    {
        //Arrange
        ManualResetEvent mre = new ManualResetEvent(false);
        var cmd = () =>
            {
                mre.Set();
            };
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        var thread2 = IoC.Resolve<ServerThread>("Create And Start Thread", "thread2", sender, receiver);
        thread2.Execute();
        //Act
        var tsc = IoC.Resolve<SpaceBattle.Lib.ICommand>("Hard Stop The Thread", "thread2", cmd);
        tsc.Execute();
        mre.WaitOne();
        //Assert
        Assert.True(thread2.IsThreadStopped());
    }
}
