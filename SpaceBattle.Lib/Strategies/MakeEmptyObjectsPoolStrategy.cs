namespace SpaceBattle.Lib;

public class MakeEmptyObjectsStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var gameid = (string)args[0];
        var pool = new Dictionary<string, IUObject>();
        for(int i = 0; i < 6; i++)
        {
            var ship = new UObject();
            pool.TryAdd(pool.Count().ToString(), ship);
            new InitPositionCommand(ship).Execute();
            new InitFuelCommand(ship).Execute();
        }
        return pool;
    }
}
