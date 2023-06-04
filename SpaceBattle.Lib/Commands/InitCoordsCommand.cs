namespace SpaceBattle.Lib;
using Hwdtech;

public class InitCoordsCommand : ICommand
{
    IEnumerable<object> objs;

    public InitCoordsCommand(IEnumerable<object> objs)
    {
        this.objs = objs;
    }

    public void Execute()
    {
        var Coords = IoC.Resolve<IEnumerator<Vector>>("Game.Generators.Coords", objs);

        foreach (object obj in objs)
        {
            IMovable movable = IoC.Resolve<IMovable>("Entities.Adapter.IMovable", obj);

            Coords.MoveNext();
            movable.Coords = Coords.Current;
        }
    }
}
