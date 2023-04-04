namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

class ExampleServer
{
    BlockingCollection<ICommand> queue;
    ReceiverAdapter reciever;
    ServerThread thread;
    public ExampleServer()
    {
        this.queue = new BlockingCollection<ICommand>(1000);
        this.reciever = new ReceiverAdapter(queue);
        this.thread = new ServerThread(reciever);
    }
    public void Execute()
    {
        thread.Execute();

        //thread.Add(new ThreadStopCommand(thread));

        queue.Add(new UpdateBehaviourCommand(thread,
        new ActionCommand(() =>
        {
            if (reciever.isEmpty())
            {
                thread.Stop();
            }
            else
            {
                thread.HandleCommand();
            }
        })
        ));
    }
}
