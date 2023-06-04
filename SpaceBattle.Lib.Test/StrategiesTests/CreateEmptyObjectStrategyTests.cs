using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Test;

public class CreateEmptyObjectStrategyTests
{
    public CreateEmptyObjectStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void PosCreateEmptyObjectStrategyTests()
    {
        //Arrange
        var strategy = new CreateEmptyObjectStrategy();
        
        //Act
        var obj = strategy.Execute();

        //Assert
        Assert.True(obj.GetType() == typeof(Dictionary<string, object>));
        Assert.True(((Dictionary<string, object>)obj).Count() == 0);
    }
}
