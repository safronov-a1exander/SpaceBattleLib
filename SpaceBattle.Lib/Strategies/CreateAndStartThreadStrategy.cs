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
        SenderAdapter sender = new SenderAdapter(commands);
        var st = new ServerThread(rec);
        if (args.Length > 1)
        {
            sender.Send(new ActionCommand((Action)args[1]));
        }
        var threads = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread");
        threads.Add(id, new List<(ServerThread, ISender)>{(st, sender)});
        return st;
    }
}
