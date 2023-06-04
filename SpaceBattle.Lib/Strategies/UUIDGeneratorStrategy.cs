namespace SpaceBattle.Lib;

public class UuidGeneratorStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        return Guid.NewGuid().ToString();
    }
}
