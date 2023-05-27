namespace SpaceBattle.Lib;


public class SetPropertyStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        IUObject patient = (IUObject)args[0];
        string property = (string)args[1];
        object value = args[2];
        return patient;
    }
}
