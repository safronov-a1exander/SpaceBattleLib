using Moq;
using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class ExceptionHandlerFindStrategyTests
{
    [Fact]
    void PosTestExceptionHandlerFindStrategy()
    {
        var Handler = new Mock<IStrategy>();

        var ExceptionStrategyDict = new Mock<IDictionary<Type, IStrategy>>();
        ExceptionStrategyDict.Setup(ed => ed[It.IsAny<Type>()]).Returns(Handler.Object);

        var HandlerDict = new Mock<IDictionary<Type, IDictionary<Type, IStrategy>>>();

        HandlerDict.Setup(md => md[It.IsAny<Type>()]).Returns(ExceptionStrategyDict.Object);

        Handler.Setup(_s => _s.Execute(It.IsAny<object[]>())).Returns(HandlerDict.Object);

        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Handler.Exception", (object[] props) => Handler.Object.Execute(props)).Execute();

        var Strat = new ExceptionHandlerFindStrategy();

        Assert.NotNull(Strat.Execute(new Mock<Type>().Object, new Mock<Type>().Object));
    }
}
