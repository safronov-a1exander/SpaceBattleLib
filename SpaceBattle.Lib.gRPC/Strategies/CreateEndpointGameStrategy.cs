namespace SpaceBattle.Lib.gRPC;
using SpaceBattle.Lib;
using Hwdtech;
using Hwdtech.Ioc;
using System.Collections.Concurrent;
public class CreateEndpointGameStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        Dictionary<string, ISender> internalDict = new();
        Dictionary<string, ISender> routeDict = new();

                IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetInternalSenderByThreadId", (object[] args) =>
        {
            return internalDict[(string)args[0]];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetOrderSenderByThreadId", (object[] args) =>
        {
            return routeDict[(string)args[0]];
        }).Execute();

        int threadsQuantity = (int)args[0];

        for (int i = 0; i < threadsQuantity; i++)
        {
            BlockingCollection<SpaceBattle.Lib.ICommand> queue = new();
            BlockingCollection<SpaceBattle.Lib.ICommand> orderQueue = new();
            ISender snd = new SenderAdapter(orderQueue);
            ISender internalSnd = new SenderAdapter(queue);
            IReceiver rec = new ReceiverAdapter(queue);
            IReceiver orderRec = new ReceiverAdapter(orderQueue);
            ServerThread thread = new(rec, orderRec);
            internalDict.Add(i.ToString(), internalSnd);
            routeDict.Add(i.ToString(), snd);
            thread.Execute();
        }

        IRouter router = new Router(routeDict);
        return router;
    }
}
