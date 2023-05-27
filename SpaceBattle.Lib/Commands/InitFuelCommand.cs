namespace SpaceBattle.Lib;
using Hwdtech;

public class InitFuelCommand : ICommand
{
    IUObject target;
    public InitFuelCommand(IUObject target)
    {
        this.target = target;
    }
    public void Execute()
    {
       var massOfFuel = IoC.Resolve<int>("RocketInitFuel"); //?
       target.setProperty("Fuel", massOfFuel);
    }
}
