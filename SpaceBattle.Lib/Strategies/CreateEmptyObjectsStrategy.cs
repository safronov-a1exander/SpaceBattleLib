namespace SpaceBattle.Lib;

public class CreateEmptyObjectStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new Dictionary<string, object>();
    }
}
