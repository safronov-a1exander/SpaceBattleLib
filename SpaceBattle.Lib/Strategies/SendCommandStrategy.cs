using Hwdtech;
namespace SpaceBattle.Lib;

public class SendCommandStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var snd = IoC.Resolve<IDictionary<string, List<(ServerThread, ISender)>>>("Storage.Thread")[(string)args[0]][0].Item2;
        var cmd = (ICommand)args[1];
        return new SendCommand(snd, cmd);
    }
}
