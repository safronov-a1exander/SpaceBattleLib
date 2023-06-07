namespace SpaceBattle.Lib.Test;
using Hwdtech;
using Hwdtech.Ioc;
using Moq;


public class MakeEmptyObjectsCommandTests
{
    [Fact]
    void PosMakeEmptyObjects()
    {
        //Arrange
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IDictionary<string, object> stor = new Dictionary<string, object>();

        IoC.Resolve<ICommand>("IoC.Register", "Game.Objects.Storage.Add", (object[] argv) =>
            {
                string id = (string)argv[0];
                object obj = argv[1];
                var storAdder = new Mock<SpaceBattle.Lib.ICommand>();
                storAdder.Setup(c => c.Execute()).Callback(() => stor.Add(id, obj));
                return storAdder.Object;
            }
        ).Execute();

        IoC.Resolve<ICommand>("IoC.Register", "System.Generator.Uuid", (object[] _) => Guid.NewGuid().ToString()).Execute();
        IoC.Resolve<ICommand>("IoC.Register", "Game.Objects.Empty", (object[] _) => new object()).Execute();

        var quantity = 5;

        //Act
        var meoc = new MakeEmptyObjectsCommand(quantity);
        meoc.Execute();

        //Assert
        Assert.Equal(quantity, stor.Count);
    }
}
