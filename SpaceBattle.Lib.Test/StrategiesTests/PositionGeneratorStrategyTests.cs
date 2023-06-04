namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;

public class PositionGeneratorStrategyTests
{
    [Fact]
    void PosPositionGeneratorStrategy()
    {
        //Arrange
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var pgs = new PositionGeneratorStrategy();

        var iterable = Enumerable.Repeat<object>(new object(), 3);
        var start = new Vector(0, 0);
        var step = new Vector(1, 0);

        var generator = (IEnumerator<Vector>)pgs.Execute(iterable, start, step);

        //Act
        generator.MoveNext();

        //Assert
        Assert.Equal<Vector>(generator.Current, new Vector(1, 0));
    }
}
