using Hwdtech;
namespace SpaceBattle.Lib;

public class HardStopTheThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var thread = IoC.Resolve<ServerThread>("Storage.GetThreadByID", args[0]);
        var threadstop = new ThreadStopCommand(thread);
        if (args.Length > 1)
        {
            var action = new ActionCommand((Action)args[1]);
            var macrostop = new MacroCommand(new List<ICommand>() { action, threadstop });
            return macrostop;
        }
        return threadstop;
    }
}
