namespace SpaceBattle.Lib;
using Hwdtech;

public class DeleteObjectFromPoolStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var gameid = (string)args[0];
        var id = (string)args[1];
        var pool = IoC.Resolve<IDictionary<string, IUObject>>("Storage.Game.IUObjects", gameid);
        if (pool.ContainsKey("id")) pool.Remove("id");
        return pool;
    }
}
