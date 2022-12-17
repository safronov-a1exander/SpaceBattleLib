using Hwdtech;

namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    IMoveCommandStartable UObject { get; }

    public StartMoveCommand(IMoveCommandStartable UObject)
    {
        this.UObject = UObject;
    }

    public void Execute()
    {
        IMovable MAdapter = IoC.Resolve<IMovable>("Adapter.Movable", this.UObject);
        ICommand MCommand = IoC.Resolve<ICommand>("Command.Move", MAdapter);
        IoC.Resolve<ICommand>("Operation.Queue.Push", MCommand, UObject.Queue);
    }
}
