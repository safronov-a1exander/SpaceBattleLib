using Moq;

namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;

public class InitPlayersCoordsStrategyTests
{
    public InitPlayersCoordsStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    void PosInitPlayersCoordsStrategy()
    {
        //Arrange
        var initCommand = new Mock<SpaceBattle.Lib.ICommand>();
        initCommand.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Objects.Commands.InitCoords", (object[] args) =>
            {
                var gameObjects = (IEnumerable<object>)args[0];
                return initCommand.Object;
            }
        ).Execute();

        var gameObjects = new List<IEnumerable<object>>{new object[]{}, new object[]{}};

        var ipps = new InitPlayersCoordsStrategy();

        //Act
        ((SpaceBattle.Lib.ICommand)ipps.Execute(gameObjects)).Execute();

        //Assert
        initCommand.Verify();
    }
}
