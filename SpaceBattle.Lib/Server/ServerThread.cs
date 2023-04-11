namespace SpaceBattle.Lib;
using Hwdtech;
public class ServerThread
{
    public Thread thread;
    public IReceiver queue;
    bool stop = false;
    ActionCommand strategy;

    internal void Stop() => stop = true;

    internal void HandleCommand()
    {
        var cmd = queue.Receive();
        try
        {
            cmd.Execute();
        }
        catch (Exception e)
        {
            IoC.Resolve<ICommand>("Handler.Exception", e, cmd);
        }

    }
    public ServerThread(IReceiver queue)
    {
        this.queue = queue;
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
}
