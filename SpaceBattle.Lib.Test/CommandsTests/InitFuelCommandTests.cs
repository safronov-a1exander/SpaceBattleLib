namespace SpaceBattleTests.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class InitFuelCommandTests
{
    [Fact]
    void PosInitFuelCommand()
    {
        //Arrange
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        int generatedCount = 0;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Generators.Fuel", (object[] _) =>
            {
                var generator = new Mock<IEnumerator<int>>();
                generator.Setup(g => g.MoveNext()).Callback(() => generatedCount++);
                generator.Setup(g => g.Current).Returns(() => generatedCount);
                return generator.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Entities.Adapter.IFuelable", (object[] argv) =>
            {
                var obj = (IDictionary<string, object>)argv[0];
                var fuelable = new Mock<IFuelable>();
                fuelable.SetupGet(f => f.fuelLevel).Returns(() => (int)obj["Fuel"]);
                fuelable.SetupSet(f => f.fuelLevel).Callback((int value) => { obj["Fuel"] = value; });
                return fuelable.Object;
            }
        ).Execute();

        var objects = new List<Dictionary<string, object>>();
        foreach (int _ in Enumerable.Range(0, 5))
        {
            objects.Add(new Dictionary<string, object>());
        }

        var isfc = new InitFuelCommand(objects.AsEnumerable());

        //Act
        isfc.Execute();

        //Assert
        Assert.Equal<object>(
            objects.Select((Dictionary<string, object> obj) => obj["Fuel"]).ToList<object>(),
            Enumerable.Range(1, generatedCount).ToList<int>()
        );
    }
}
