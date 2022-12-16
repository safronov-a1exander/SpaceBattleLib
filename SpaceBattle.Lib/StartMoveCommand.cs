using Hwdtech;

namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    IMoveCommandStartable Uobject { get; }

    public StartMoveCommand(IMoveCommandStartable Uobject)
    {
        this.Uobject = Uobject;
    }

    public void Execute()
    {
        IMovable MAdapter = IoC.Resolve<IMovable>("Adapter.Movable", this.Uobject);
        ICommand MCommand = IoC.Resolve<ICommand>("Command.Move", MAdapter);
        IoC.Resolve<ICommand>("Operation.Queue.Push", MCommand, Uobject.Queue);
    }
}
