namespace SpaceBattle.Lib;
using Hwdtech;
public class ServerThread
{
    Thread thread;
    IReceiver queue;
    IReceiver externalQueue;
    bool stop = false;
    ActionCommand strategy;

    internal void Stop() => stop = true;

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        var order = externalQueue.Receive();
        try
        {
            cmd.Execute();
            order.Execute();
        }
        catch (Exception e)
        {
            IoC.Resolve<ICommand>("Handler.Exception", e, cmd);
        }

    }
    public ServerThread(IReceiver queue, IReceiver externalQueue)
    {
        this.queue = queue;
        this.externalQueue = externalQueue;
        strategy = new ActionCommand(() =>
        {
            HandleCommand();
        });
        this.thread = new Thread(() =>
        {
            while (!stop)
            {
                strategy.Execute();
            }
        });
    }

    internal void UpdateBehaviour(ActionCommand newBehaviour)
    {
        strategy = newBehaviour;

    }
    public void Execute()
    {
        thread.Start();
    }
    public bool IsReceiverEmpty()
    {
        return queue.isEmpty();
    }
    public bool IsThreadStopped()
    {
        return stop;
    }
}
