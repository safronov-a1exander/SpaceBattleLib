using Moq;
using FluentAssertions;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CollisionCheckTests
{
    [Fact]
    public void PosTestCollisionCheck()
    {
        var command = new Mock<ICommand>();
        command.Setup(_c => _c.Execute());

        var NoPropertyStrategy = new Mock<IStrategy>();
        NoPropertyStrategy.Setup(_n => _n.Execute()).Returns(command.Object);

        var PropertyStrategy = new Mock<IStrategy>();
        PropertyStrategy.Setup(_s => _s.Execute(It.IsAny<object[]>())).Returns(command.Object).Verifiable();

        var Dict = new Mock<IDictionary<int, object>>();
        Dict.Setup(_d => _d.Keys).Returns(new List<int> { 1 });

        var DictStrategy = new Mock<IStrategy>();
        DictStrategy.Setup(_d => _d.Execute()).Returns(Dict.Object).Verifiable();

        var ListStrategy = new Mock<IStrategy>();
        ListStrategy.Setup(_l => _l.Execute(It.IsAny<object[]>())).Returns(new List<int>()).Verifiable();

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get.SolutionTree", (object[] properties) => DictStrategy.Object.Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Event.Collision", (object[] properties) => PropertyStrategy.Object.Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.CalculateDeltas", (object[] properties) => ListStrategy.Object.Execute(properties)).Execute();

        var patient1 = new Mock<IUObject>();
        var patient2 = new Mock<IUObject>();

        var checker = new CollisionCheck(patient1.Object, patient2.Object);

        checker.Execute();

        PropertyStrategy.VerifyAll();
        DictStrategy.VerifyAll();
        ListStrategy.VerifyAll();
    }
}
