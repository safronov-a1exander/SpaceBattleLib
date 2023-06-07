namespace SpaceBattle.Lib;
using Hwdtech;

public class InitFuelCommand : ICommand
{
    IEnumerable<object> objs;

    public InitFuelCommand(IEnumerable<object> objs)
    {
        this.objs = objs;
    }

    public void Execute()
    {
        var fuel = IoC.Resolve<IEnumerator<int>>("Game.Generators.Fuel", objs);

        foreach (object obj in objs)
        {
            IFuelable fuelable = IoC.Resolve<IFuelable>("Entities.Adapter.IFuelable", obj);

            fuel.MoveNext();
            fuelable.fuelLevel = fuel.Current;
        }
    }
}
