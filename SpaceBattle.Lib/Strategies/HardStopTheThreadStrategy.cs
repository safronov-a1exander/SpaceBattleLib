using Hwdtech;
namespace SpaceBattle.Lib;

public class HardStopTheThreadStrategy: IStrategy
{
    public object Execute(params object[] args)
    {
        var thread = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread")[(string)args[0]][0].Item1;
        if (args.Length > 1)
        {
            var action = new ActionCommand((Action)args[1]);
            thread.UpdateBehaviour(action);
        }
        thread.UpdateBehaviour(new ActionCommand((Action)args[1]));
        var tsc = new ThreadStopCommand(thread);
        tsc.Execute();
        return 0;
    }
}
