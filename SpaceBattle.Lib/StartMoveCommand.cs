using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    IMoveCommandStartable installator { get; }

    public StartMoveCommand(IMoveCommandStartable UObject)
    {
        installator = UObject;
    }

    public void Execute()
    {
        installator.action.ToList().ForEach(o => IoC.Resolve<ICommand>("General.SetProperty", installator.UObject, o.Key, o.Value).Execute());
        ICommand MCommand = IoC.Resolve<ICommand>("Command.Move", installator.UObject);
        IoC.Resolve<ICommand>("General.SetProperty", installator.UObject, "Commands.Movement", MCommand).Execute();
        IoC.Resolve<ICommand>("Queue.Push", IoC.Resolve<Queue<ICommand>>("Queue.Main"), MCommand).Execute();
    }
}
