namespace SpaceBattle.Lib;
using Hwdtech;
public class MakeEmptyObjectsCommand : ICommand
{
    int quantity;

    public MakeEmptyObjectsCommand(int quantity)
    {
        this.quantity = quantity;
    }

    public void Execute()
    {
        foreach (int _ in Enumerable.Range(0, quantity))
        {
            IoC.Resolve<ICommand>(
                "Game.Objects.Storage.Add",
                IoC.Resolve<string>("System.Generator.Uuid"),
                IoC.Resolve<object>("Game.Objects.Empty")
            ).Execute();
        }
    }
}
