using System.Collections.Concurrent;
using Hwdtech;
namespace SpaceBattle.Lib;

public class CreateAndStartThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var id = (string)args[0];
        BlockingCollection<ICommand> commands = new BlockingCollection<ICommand>(100);
        ReceiverAdapter rec = new ReceiverAdapter(commands);
        SenderAdapter send = new SenderAdapter(commands);
        var st = new ServerThread(rec);
        if (args.Length > 1)
        {
            var action = new ActionCommand((Action)args[1]);
            st.UpdateBehaviour(action);
        }
        var threads = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread");
        threads.Add(id, new List<(ServerThread, ISender)>{(st, send)});
        return st;
    }
}
