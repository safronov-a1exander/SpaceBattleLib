namespace SpaceBattle.Lib.Test;
using Moq;
using Hwdtech;
using Hwdtech.Ioc;

public class GameCommandTests
{
    static GameCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void PosTest()
    {
        
    }
}
