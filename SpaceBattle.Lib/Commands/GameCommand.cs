using System.Diagnostics;
using Hwdtech;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    Queue<ICommand> queue = new Queue<ICommand>(100);

    object scope;
    public GameCommand(string id, object scope)
    {
        this.scope = scope; // 1
    }

    public void Execute()
    {
        var sw = new Stopwatch();
        IoC.Resolve<ICommand>("Scope.Current.Set", scope).Execute();
        var quant = IoC.Resolve<TimeSpan>("Game.GetQuant");
        while(sw.Elapsed < quant) // 2
        {
            sw.Start();
            var success = queue.TryDequeue(out var cmd);
            try
            {               
                if (success) cmd!.Execute();
                sw.Stop();
            }
            catch (Exception e)
            {
                sw.Stop();
                IoC.Resolve<ICommand>("Handler.Exception", e, cmd!).Execute(); // 3
                throw (Exception)(e.Data["cmd"] = cmd!); // 4
            }
        }
        var sender = IoC.Resolve<string>("Storage.GetThreadByGameID");
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", sender, this).Execute();
    }
}
