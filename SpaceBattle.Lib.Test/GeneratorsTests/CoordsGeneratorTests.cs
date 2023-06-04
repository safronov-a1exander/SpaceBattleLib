namespace SpaceBattleTests.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;
using SpaceBattle.Lib;

public class CoordsGeneratorTests
{
    public CoordsGeneratorTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    void PosCoordsGeneratorTraverse()
    {
        var start = new Vector(0, 0);
        var step = new Vector(1, 2);
        var count = 3;

        var pg = new CoordsGenerator(count, start, step);

        Vector current = start;

        // Action
        while (pg.MoveNext())
        {
            current += step;
            Assert.Equal(current, pg.Current);
            Assert.Equal(current, ((IEnumerator)pg).Current);
        }

    }

    [Fact]
    void PosCoordsGeneratorReset()
    {
        //Arrange
        var start = new Vector(0, 0);
        var step = new Vector(1, 2);
        var count = 1;

        var pg = new CoordsGenerator(count, start, step);

        Vector current = start;

        //Act
        pg.MoveNext();
        var first = pg.Current;
        pg.Reset();
        pg.MoveNext();
        var second = pg.Current;

        //Assert
        Assert.Equal(first, second);
    }

    [Fact]
    void PosCoordsGeneratorDispose()
    {
        //Arrange
        var start = new Vector(0, 0);
        var step = new Vector(1, 2);
        var count = 1;

        var pg = new CoordsGenerator(count, start, step);

        //Act & Assert
        pg.Dispose();
    }

    [Fact]
    void PosCoordsGeneratorEndCheck()
    {
        //Arrange
        var start = new Vector(0, 0);
        var step = new Vector(1, 2);
        var count = 0;

        var pg = new CoordsGenerator(count, start, step);

        //Act & Assert
        Assert.False(pg.MoveNext());
    }
}
