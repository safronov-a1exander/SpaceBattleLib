using System.Collections.Concurrent;
using Hwdtech;
namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var id = (string)args[0];
        BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>(100);
        ReceiverAdapter queue = new ReceiverAdapter(commands);
        var st = new ServerThread(queue);
        if (args.Length > 1)
        {
            var action = new ActionCommand((Action)args[1]);
            st.UpdateBehaviour(action);
        }
        var threads = IoC.Resolve<IDictionary<string, ServerThread>>("Storage.Thread");
        threads.Add(id, st);
        return threads;
    }
}
