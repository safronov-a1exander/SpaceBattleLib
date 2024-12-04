using System.Collections.Concurrent;
using Hwdtech;
namespace SpaceBattle.Lib;

public class CreateThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var id = (string)args[0];
        var sender = (ISender)args[1];
        var reciever = (IReceiver)args[2];
        var externalReciever = (IReceiver)args[3];
        var serverthread = new ServerThread(reciever, externalReciever);
        if (args.Length > 4)
        {
            sender.Send(new ActionCommand((Action)args[4]));
        }
        var threadsdict = IoC.Resolve<ConcurrentDictionary<string, ServerThread>>("Storage.ThreadByID");
        threadsdict.TryAdd(id, serverthread);
        var sendersdict = IoC.Resolve<ConcurrentDictionary<string, ISender>>("Storage.ISenderByID");
        sendersdict.TryAdd(id, sender);
        return serverthread;
    }
}
