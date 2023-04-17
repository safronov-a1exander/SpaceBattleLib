using Hwdtech;
namespace SpaceBattle.Lib;

public class SoftStopTheThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var sender = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread")[(string)args[0]][0].Item2;
        var hss = IoC.Resolve<ICommand>("Hard Stop The Thread", args);
        sender.Send(hss);
        return hss;
    }
}
