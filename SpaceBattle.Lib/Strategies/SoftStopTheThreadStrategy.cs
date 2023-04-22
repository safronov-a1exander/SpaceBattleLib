using Hwdtech;
namespace SpaceBattle.Lib;

public class SoftStopTheThreadStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var sender = IoC.Resolve<ISender>("Storage.GetISenderByID", args[0]);
        var hss = IoC.Resolve<ICommand>("Hard Stop The Thread", args);
        sender.Send(hss);
        return hss;
    }
}
