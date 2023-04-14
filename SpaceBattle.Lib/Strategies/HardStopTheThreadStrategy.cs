using Hwdtech;
namespace SpaceBattle.Lib;

public class HardStopTheThreadStrategy: IStrategy
{
    public object Execute(params object[] args)
    {
        var thread = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread")[(string)args[0]][0].Item1;
        if (args.Length > 1)
        {
            thread.UpdateBehaviour(new ActionCommand((Action)args[1]));
        }
        return new ThreadStopCommand(thread);
    }
}
