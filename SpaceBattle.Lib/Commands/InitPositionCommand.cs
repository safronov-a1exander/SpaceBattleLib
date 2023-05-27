namespace SpaceBattle.Lib;
using Hwdtech;

public class InitPositionCommand : ICommand
{
    IUObject target;
    public InitPositionCommand(IUObject target)
    {
        this.target = target;
    }
    public void Execute()
    {
       var coords = IoC.Resolve<Vector>("RocketInitPositionIter"); //?
       target.setProperty("Coords", coords);
       //IoC.Resolve<ICommand>("IUObject.SetProperty", target, "Coords", coords).Execute();
    }
}
