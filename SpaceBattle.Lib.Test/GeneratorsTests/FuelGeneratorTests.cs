namespace SpaceBattleTests.Lib.Test;
using SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections;


public class FuelGeneratorTests
{
    public FuelGeneratorTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        var fuelDict = new Dictionary<object, int>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Game.Storage.FuelLevel",(object[] argv) => fuelDict).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register","Game.Objects.Properties.Get",(object[] argv) =>
            {
                var propName = (string)argv[0];
                var obj = argv[1];
                if (propName == "Fuel")
                    return (object)fuelDict[obj];
                else throw new Exception();
            }
        ).Execute();
    }

    [Fact]
    void PosFuelGeneratorTraverse()
    {
        //Arrange
        var objects = new List<object>{
            new object(),
            new object(),
            new object()
        };
        var fuelDict = IoC.Resolve<Dictionary<object, int>>("Game.Storage.FuelLevel");
        foreach (object obj in objects)
        {
            fuelDict[obj] = Random.Shared.Next();
        }

        var fg = new FuelGenerator(objects);

        //Act & Assert
        foreach (object obj in objects)
        {
            fg.MoveNext();
            Assert.Equal(fuelDict[obj], fg.Current);
            Assert.Equal(fuelDict[obj], ((IEnumerator)fg).Current);
        }
    }

    [Fact]
    void PosFuelGeneratorReset()
    {
        //Arrange
        var objects = new List<object>{
            new object()
        };

        var fuelDict = IoC.Resolve<Dictionary<object, int>>("Game.Storage.FuelLevel");

        foreach (object obj in objects)
        {
            fuelDict[obj] = Random.Shared.Next();
        }

        var fg = new FuelGenerator(objects);

        //Act
        fg.MoveNext();
        var first = fg.Current;
        fg.Reset();
        fg.MoveNext();
        var second = fg.Current;

        //Assert
        Assert.Equal(first, second);
    }

    [Fact]
    void PosFuelGeneratorDispose()
    {
        //Arrange
        var fg = new FuelGenerator(new object[] { });

        //Act & Assert
        fg.Dispose();
    }

    [Fact]
    void PosFuelGeneratorEndCheck()
    {
        //Arrange
        var objects = new List<object> { };

        var fg = new FuelGenerator(objects);

        //Act & Assert
        Assert.False(fg.MoveNext());
    }
}
