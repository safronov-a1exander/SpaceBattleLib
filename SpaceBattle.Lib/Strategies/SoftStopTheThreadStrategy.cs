using Hwdtech;
namespace SpaceBattle.Lib;

public class SoftStopTheThreadStrategy: IStrategy
{
    public object Execute(params object[] args)
    {
        var thread = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread")[(string)args[0]][0].Item1;
        var tsc = new ThreadStopCommand(thread);
        while (!thread.queue.isEmpty())
        {
            continue;
        }
        if (args.Length > 1)
        {
            var action = new ActionCommand((Action)args[1]);
            thread.UpdateBehaviour(action);
        }
        tsc.Execute();
        return 0;
    }
}
