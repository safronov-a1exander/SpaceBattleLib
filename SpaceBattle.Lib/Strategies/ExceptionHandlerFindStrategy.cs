namespace SpaceBattle.Lib;

using Hwdtech;

public class ExceptionHandlerFindStrategy : IStrategy
{
    public object Execute(params object[] argv)
    {
        Type command = (Type)argv[0];
        Type exception = (Type)argv[1];

        var ExceptionHandlers = IoC.Resolve<IDictionary<Type, IDictionary<Type, IStrategy>>>("Handler.Exception");

        return ExceptionHandlers[command][exception];
    }
}
