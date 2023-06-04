namespace SpaceBattle.Lib;
using Hwdtech;

public class InitPositionCommand : ICommand
{
    IEnumerable<object> objs;

    public InitPositionCommand(IEnumerable<object> objs)
    {
        this.objs = objs;
    }

    public void Execute()
    {
        var Coords = IoC.Resolve<IEnumerator<Vector>>("Game.Generators.Position", objs);

        foreach (object obj in objs)
        {
            IMovable movable = IoC.Resolve<IMovable>("Entities.Adapter.IMovable", obj);

            Coords.MoveNext();
            movable.Coords = Coords.Current;
        }
    }
}
