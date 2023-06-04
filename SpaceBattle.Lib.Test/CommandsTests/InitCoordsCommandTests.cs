namespace SpaceBattleTests.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class InitGameObjectsCoordsCommandTests
{

    [Fact]
    void InitGameObjectsCoords_Successful()
    {
        //Arrange
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Vector generatedVector = new Vector(0);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Generators.Coords", (object[] _) =>
            {
                var generator = new Mock<IEnumerator<Vector>>();
                generator.Setup(g => g.MoveNext()).Callback(() => generatedVector[0]++);
                generator.Setup(g => g.Current).Returns(() => generatedVector);
                return generator.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Entities.Adapter.IMovable", (object[] argv) =>
            {
                var obj = (IDictionary<string, object>)argv[0];
                var movable = new Mock<IMovable>();
                movable.SetupGet(m => m.Coords).Returns(() => (Vector)obj["Coords"]);
                movable.SetupSet(m => m.Coords = It.IsAny<Vector>()).Callback((Vector value) => { obj["Coords"] = value; });
                return movable.Object;
            }
        ).Execute();

        var objects = Enumerable.Repeat(new Dictionary<string, object>(), 5);

        var ipc = new InitCoordsCommand(objects);

        // Action
        ipc.Execute();

        // Assertation
        foreach (IDictionary<string, object> obj in Enumerable.Reverse(objects))
        {
            Assert.True((obj["Coords"].Equals(generatedVector)));
            generatedVector[0]--;
        }
    }
}
