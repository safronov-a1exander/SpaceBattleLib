using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class EndMoveCommandTests
{
    public EndMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var CommandMock = new Mock<ICommand>();
        var regStrategy = new Mock<IStrategy>();
        regStrategy.Setup(_s => _s.Execute(It.IsAny<object[]>())).Returns(CommandMock.Object);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.EmptyCommand", (object[] args) => regStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.DeleteProperty", (object[] args) => regStrategy.Object.Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.InjectCommand", (object[] args) => regStrategy.Object.Execute(args)).Execute();
    }
    [Fact]
    public void EndMoveCommandTest()
    {
        var mve = new Mock<IMoveCommandEndable>();
        mve.SetupGet(x => x.MoveCommand).Returns(new Mock<ICommand>().Object);
        mve.SetupGet(x => x.UObject).Returns(new Mock<IUObject>().Object);
        mve.SetupGet(x => x.Queue).Returns(new Mock<IQueue<ICommand>>().Object);
        ICommand EndMoveCommand = new EndMoveCommand(mve.Object);
        EndMoveCommand.Execute();
        mve.VerifyAll();
    }
}
