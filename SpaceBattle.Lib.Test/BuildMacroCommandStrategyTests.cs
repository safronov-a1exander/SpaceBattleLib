using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class BuildMacroCommandStrategyTests
{
    public BuildMacroCommandStrategyTests()
    {
        var cmd = new Mock<ICommand>();
        cmd.Setup(_c => _c.Execute());
        var PropStrat = new Mock<IStrategy>();
        PropStrat.Setup(_s => _s.Execute(It.IsAny<object[]>())).Returns(cmd.Object);
        var ListStrat = new Mock<IStrategy>();
        ListStrat.Setup(_i => _i.Execute()).Returns(new string[] { "Second" });

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Operations.First", (object[] props) => ListStrat.Object.Execute()).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Second", (object[] props) => PropStrat.Object.Execute(props)).Execute();
    }
    [Fact]
    public void PosBuildMacroCommandStrategyTests()
    {
        var patient = new Mock<IUObject>();
        var creator = new BuildMacroCommandStrategy();

        var mc = (ICommand)creator.Execute("First", patient.Object);

        mc.Execute();
    }
}
