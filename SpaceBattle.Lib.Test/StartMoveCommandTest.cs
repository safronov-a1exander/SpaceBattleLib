namespace SpaceBattle.Lib.Test;
using Hwdtech.Ioc;
using Moq;
using FluentAssertions;

public class StartMoveCommandTest
{
    static void StartMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", Hwdtech.IoC.Resolve<object>("Scopes.New", Hwdtech.IoC.Resolve<object>("Scopes.Root"))).Execute();
        
        var mockCommand = new Mock<ICommand>();
        mockCommand.Setup(x => x.Execute());

        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Adapter.Movable", (object[] args) => new Mock<ICommand>().Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.Move", (object[] args) => new Mock<ICommand>().Object).Execute();
        Hwdtech.IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operation.Queue.Push", (object[] args) => new Mock<ICommand>().Object).Execute();
    }

    [Fact]
    public void PosTestStartMoveCommand()
    {
        var m = new Mock<IMoveCommandStartable>();
        m.SetupGet(a => a.UObject).Returns(new Mock<IUObject>().Object).Verifiable();
        m.SetupGet(a => a.Speed).Returns(new Vector(It.IsAny<int>(), It.IsAny<int>())).Verifiable();
        m.SetupGet(a => a.Queue).Returns(new Mock<IQueue<ICommand>>().Object).Verifiable();
        ICommand startMoveCommand = new StartMoveCommand(m.Object);
        startMoveCommand.Execute();
        m.Verify();
    }
    [Fact]
    public void NegTestStartMoveCommand_UnableToGetUObject()
    {
        var m = new Mock<IMoveCommandStartable>();
        m.SetupGet(_m => _m.UObject).Throws<NullReferenceException>();
        var c = new StartMoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<NullReferenceException>();
    }
    [Fact]
    public void NegTestMove_UnableToGetSpeed()
    {
        var m = new Mock<IMovable>();
        m.SetupGet(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.SetupGet(_m => _m.Speed).Throws<NullReferenceException>();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<NullReferenceException>();
    }
    [Fact]
    public void NegTestMove_UnableToAdd()
    {
        var m = new Mock<IMovable>();
        m.SetupGet(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.SetupGet(_m => _m.Speed).Returns(new Vector(-7, 3, 1)).Verifiable();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<ArgumentException>();
    }
}
