namespace SpaceBattle.Lib;

using Hwdtech;

public class ExceptionHandlerFindStrategy : IStrategy
{
    public object Execute(params object[] argv)
    {
        Type command = argv[0].GetType();
        Type exception = argv[1].GetType();

        var ExceptionHandlers = IoC.Resolve<IDictionary<Type, IDictionary<Type, IStrategy>>>("Handler.Exception");

        return ExceptionHandlers[command][exception];
    }
}
