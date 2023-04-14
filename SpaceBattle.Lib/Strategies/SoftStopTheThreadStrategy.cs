using Hwdtech;
namespace SpaceBattle.Lib;

public class SoftStopTheThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var thread = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread")[(string)args[0]][0].Item1;
        while (!thread.queue.isEmpty())
        {
            continue;
        }
        if (args.Length > 1)
        {
            thread.UpdateBehaviour(new ActionCommand((Action)args[1]));
        }
        return new ThreadStopCommand(thread);
    }
}
