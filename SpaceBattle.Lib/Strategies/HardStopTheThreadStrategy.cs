using Hwdtech;
namespace SpaceBattle.Lib;

public class HardStopTheThreadStrategy: IStrategy
{
    public object Execute(params object[] args)
    {
        var threads = IoC.Resolve<IDictionary<string, ServerThread>>("Storage.Thread");
        
        return 0;
    }
}