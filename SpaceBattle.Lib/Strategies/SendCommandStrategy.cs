using Hwdtech;
namespace SpaceBattle.Lib;

public class SendCommandStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var sender = IoC.Resolve<ISender>("Storage.GetISenderByID", args[0]);
        var cmd = (ICommand)args[1];
        return new SendCommand(sender, cmd);
    }
}
