namespace SpaceBattle.Lib;

public class UObject : IUObject
{
    Dictionary<string, object> Values;

    public UObject()
    {
        Values = new Dictionary<string, object>();
    }
        
    public object getProperty(string key)
    {
        return Values[key];
    }

    public void setProperty(string key, object value)
    {
        Values.Add(key, value);
    }
}
