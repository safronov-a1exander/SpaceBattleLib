using System.Diagnostics;
using Hwdtech;
namespace SpaceBattle.Lib;

public class GameCommand : ICommand
{
    Queue<ICommand> queue = new Queue<ICommand>(100);

    object scope;
    public GameCommand(string id, object scope)
    {
        this.scope = scope;
    }

    public void Execute()
    {
        var sw = new Stopwatch();
        IoC.Resolve<ICommand>("Scope.Current.Set", scope).Execute();
        var quant = IoC.Resolve<TimeSpan>("GetQuant");
        while(sw.Elapsed < quant)
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
                IoC.Resolve<ICommand>("Handler.Exception", e, cmd!).Execute();
                throw (Exception)(e.Data["cmd"] = cmd!);
            }
        }
    }
}
