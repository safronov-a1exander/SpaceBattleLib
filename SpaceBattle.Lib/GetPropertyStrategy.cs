namespace SpaceBattle.Lib;


public class GetPropertyStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        IUObject patient = (IUObject)args[0];
        string property = (string)args[1];
        return patient.getProperty(property);
    }
}
