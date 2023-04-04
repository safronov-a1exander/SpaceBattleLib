namespace SpaceBattle.Lib.Test;
using System.Collections.Concurrent;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class ServerTests
{
    [Fact]
    public void MyThread_1()
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
    public void MyThread_2()
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

    public void DependenciesRegistrator()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var threadsdict = new Dictionary<string, ServerThread>();
        var start = new CreateAndStartThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateAndStartThread", (object[] args) => start.Execute(args)).Execute();
        var send = new SendCommandStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) => send.Execute(args)).Execute();
        var hardstop = new HardStopTheThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Hard Stop The Thread", (object[] args) => hardstop.Execute(args)).Execute();
        var softstop = new SoftStopTheThreadStrategy();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Soft Stop The Thread", (object[] args) => softstop.Execute(args)).Execute();
    }

    [Fact]
    public void MyThread_StrategyCreateAndRun()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var Th1 = IoC.Resolve<ServerThread>("CreateAndStartThread", "sample_id");
        var Th2 = IoC.Resolve<ServerThread>("CreateAndStartThread", "sample_id", (() => Thread.Sleep(5000)));
        Assert.False(Th1 == Th2);
        Assert.False(Th1.Equals(Th2));
    }
}
