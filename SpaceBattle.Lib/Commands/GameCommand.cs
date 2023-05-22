using System.Diagnostics;
using Hwdtech;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    Queue<ICommand> queue = new Queue<ICommand>(100);
    string id;

    Stopwatch sw = new Stopwatch();
    public GameCommand(string id)
    {
        this.id = id; // 1
    }

    public void Execute()
    {
        var previous_scope = IoC.Resolve<object>("Scopes.Current");
        var scope = IoC.Resolve<object>("Storage.GetScopeByGameID");
        IoC.Resolve<ICommand>("Scopes.Current.Set", scope).Execute();

        var quant = IoC.Resolve<TimeSpan>("Game.GetQuant");
        sw.Reset();
        sw.Start();

        while(sw.Elapsed < quant) // 2
        {
            var success = queue.TryDequeue(out var cmd);
            try
            {               
                if (success) cmd!.Execute();
            }
            catch (Exception e)
            {
                IoC.Resolve<ICommand>("Handler.Exception", e, cmd!).Execute(); // 3
                throw (Exception)(e.Data["cmd"] = cmd!); // 4
            }
        }
        sw.Stop();

        var sender = IoC.Resolve<string>("Storage.GetThreadByGameID");
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", sender, this).Execute();

        IoC.Resolve<ICommand>("Scopes.Current.Set", previous_scope).Execute();
    }

    public bool IsExecuted()
    {
        if (sw.ElapsedTicks > 0) return true;
        return false;
    }
}
