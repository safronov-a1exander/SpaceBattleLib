namespace SpaceBattle.Lib;

public class EmptyObjectCreateStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return new Dictionary<string, object>();
    }
}
