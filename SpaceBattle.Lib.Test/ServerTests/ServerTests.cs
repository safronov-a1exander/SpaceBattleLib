namespace SpaceBattle.Lib.Test;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class ServerTests
{
    static ServerTests()
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
        var hardstopstrat = new HardStopTheThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Hard Stop The Thread", (object[] args) => hardstopstrat.Execute(args)).Execute();
        var softstopstrat = new SoftStopTheThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) => softstopstrat.Execute(args)).Execute();

    }

    [Fact]
    public void PosTestServerThreadWithMRE()
    {
        ManualResetEvent mre = new ManualResetEvent(false);
        BlockingCollection<SpaceBattle.Lib.ICommand> queue = new BlockingCollection<SpaceBattle.Lib.ICommand>();
        var receiver = new Mock<IReceiver>();
        receiver.Setup(r => r.Receive()).Returns(() => queue.Take());
        receiver.Setup(r => r.isEmpty()).Returns(() => queue.Count() == 0);
        var sender = new Mock<ISender>();
        sender.Setup(s => s.Send(It.IsAny<SpaceBattle.Lib.ICommand>())).Callback<SpaceBattle.Lib.ICommand>((c) => queue.Add(c));
        Assert.True(receiver.Object.isEmpty());
        var cmd1 = new ActionCommand(() =>
        {
            Thread.Sleep(5000);
        });
        var cmd2 = new ActionCommand(
            () =>
            {
                Thread.Sleep(1);
            }
        );
        var cmd3 = new ActionCommand(
            () =>
            {
                mre.Set();
            }
        );

        sender.Object.Send(cmd1);
        sender.Object.Send(cmd2);
        sender.Object.Send(cmd3);

        Assert.False(receiver.Object.isEmpty());

        ServerThread mt = new ServerThread(receiver.Object);
        mt.Execute();

        mre.WaitOne();

        Assert.True(receiver.Object.isEmpty());
    }

    [Fact]
    public void PosTestServerThreadWithBarrier()
    {
        Barrier barrier = new Barrier(3);

        BlockingCollection<SpaceBattle.Lib.ICommand> queue1 = new BlockingCollection<SpaceBattle.Lib.ICommand>();

        var receiver1 = new Mock<IReceiver>();
        receiver1.Setup(r => r.Receive()).Returns(() => queue1.Take());
        receiver1.Setup(r => r.isEmpty()).Returns(() => queue1.Count() == 0);

        var sender1 = new Mock<ISender>();
        sender1.Setup(s => s.Send(It.IsAny<SpaceBattle.Lib.ICommand>())).Callback<SpaceBattle.Lib.ICommand>((c) => queue1.Add(c));

        BlockingCollection<SpaceBattle.Lib.ICommand> queue2 = new BlockingCollection<SpaceBattle.Lib.ICommand>();

        var receiver2 = new Mock<IReceiver>();
        receiver2.Setup(r => r.Receive()).Returns(() => queue2.Take());
        receiver2.Setup(r => r.isEmpty()).Returns(() => queue2.Count() == 0);

        var sender2 = new Mock<ISender>();
        sender2.Setup(s => s.Send(It.IsAny<SpaceBattle.Lib.ICommand>())).Callback<SpaceBattle.Lib.ICommand>((c) => queue2.Add(c));

        Assert.True(receiver1.Object.isEmpty());
        Assert.True(receiver2.Object.isEmpty());

        var cmd1 = new ActionCommand(() =>
        {
            Thread.Sleep(5000);
        });
        var cmd2 = new ActionCommand(
            () =>
            {
                Thread.Sleep(1);
            }
        );
        var cmd3 = new ActionCommand(
            () =>
            {
                barrier.SignalAndWait();
            }
        );
        var cmd4 = new ActionCommand(
            () =>
            {
                barrier.SignalAndWait();
            }
        );

        sender1.Object.Send(cmd1);
        sender1.Object.Send(cmd2);
        sender1.Object.Send(cmd3);

        sender2.Object.Send(cmd2);
        sender2.Object.Send(cmd1);
        sender2.Object.Send(cmd4);

        Assert.False(receiver1.Object.isEmpty());

        ServerThread mt1 = new ServerThread(receiver1.Object);
        mt1.Execute();

        ServerThread mt2 = new ServerThread(receiver2.Object);
        mt2.Execute();

        barrier.SignalAndWait();

        Assert.True(receiver1.Object.isEmpty());
        Assert.True(receiver2.Object.isEmpty());
    }

    [Fact]
    public void PosTestCreateThreadStrategy()
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
        //Act
        var thread1 = IoC.Resolve<ServerThread>("Create Thread", "thread1", sender, receiver, cmd);
        //Assert
        Assert.True(IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Storage.ThreadByID").ContainsKey("thread1"));
        Assert.True(IoC.Resolve<ConcurrentDictionary<string, ISender>>("Storage.ISenderByID").ContainsKey("thread1"));
        Assert.False(thread1.IsReceiverEmpty());
        thread1.Execute();
        mre.WaitOne();
        Assert.True(thread1.IsReceiverEmpty());
    }

    [Fact]
    public void PosTestHardStopTheThreadStrategy()
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
        var thread2 = IoC.Resolve<ServerThread>("Create Thread", "thread2", sender, receiver);
        thread2.Execute();
        //Act
        var tsc = IoC.Resolve<SpaceBattle.Lib.ICommand>("Hard Stop The Thread", "thread2", cmd);
        tsc.Execute();
        mre.WaitOne();
        //Assert
        Assert.True(thread2.IsThreadStopped());
    }

    [Fact]
    public void PosTestSoftStopTheStrategy()
    {
        //Arrange
        ManualResetEvent mre1 = new ManualResetEvent(false);
        ManualResetEvent mre2 = new ManualResetEvent(false);
        var cmd1 = () =>
            {
                mre1.Set();
            };
        var cmd2 = () =>
            {
                mre2.Set();
            };
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        //Act
        var thread3 = IoC.Resolve<ServerThread>("Create Thread", "thread3", sender, receiver);
        sender.Send(new ActionCommand(cmd1));
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Soft Stop The Thread", "thread3", cmd2).Execute();
        thread3.Execute();
        //Assert
        mre1.WaitOne();
        mre2.WaitOne(10000);
        Assert.True(thread3.IsThreadStopped());
    }

    [Fact]
    public void PosTestSendCommandStrategy()
    {
        //Arrange
        ManualResetEvent mre = new ManualResetEvent(false);
        var queue = new BlockingCollection<SpaceBattle.Lib.ICommand>(100);
        var receiver = new ReceiverAdapter(queue);
        var sender = new SenderAdapter(queue);
        var thread4 = IoC.Resolve<ServerThread>("Create Thread", "thread4", sender, receiver);
        thread4.Execute();
        //Act
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", "thread4", new ActionCommand(() => mre.Set())).Execute();
        //Assert
        mre.WaitOne();
    }

}
