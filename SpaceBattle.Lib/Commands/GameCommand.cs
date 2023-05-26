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
            }
        }
        sw.Stop();
        
    }
}
