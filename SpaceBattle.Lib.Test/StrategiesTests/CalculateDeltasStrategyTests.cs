using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CalculateDeltasStratTests
{
    public CalculateDeltasStratTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "UObject.GetProperty", (object[] properties) => new GetPropertyStrategy().Execute(properties)).Execute();
    }
    [Fact]
    public void PosTestCalculateDeltas()
    {
        var patient1 = new Mock<IUObject>();
        patient1.Setup(_p => _p.getProperty("Speed")).Returns(new Vector(1, 1));
        patient1.Setup(_p => _p.getProperty("Coords")).Returns(new Vector(1, 2));
        var patient2 = new Mock<IUObject>();
        patient2.Setup(_p => _p.getProperty("Speed")).Returns(new Vector(-1, -1));
        patient2.Setup(_p => _p.getProperty("Coords")).Returns(new Vector(4, 5));

        var preparator = new CalculateDeltas();

        var deltas = preparator.Execute(patient1.Object, patient2.Object);

        Assert.Equal(new List<int> { -3, -3, 2, 2 }, deltas);
    }
}
