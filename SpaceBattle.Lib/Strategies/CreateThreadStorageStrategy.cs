namespace SpaceBattle.Lib;
using Hwdtech;
public class CreateThreadStorageStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var dict = new Dictionary<string, List<(ServerThread, ISender)>>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Storage.Thread", (object[] obj) => dict).Execute();
        return dict;
    }
}
