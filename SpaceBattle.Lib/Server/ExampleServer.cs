namespace SpaceBattle.Lib;
using System.Collections.Concurrent;

class ExampleServer
{
    BlockingCollection<ICommand> queue;
    RecieverAdapter reciever;
    MyThread thread;
    public ExampleServer()
    {
        this.queue = new BlockingCollection<ICommand>(1000);
        this.reciever = new RecieverAdapter(queue);
        this.thread = new MyThread(new RecieverAdapter(queue));
    }
    public void Execute()
    {
        thread.Execute();

        //thread.Add(new ThreadStopCommand(thread));

        queue.Add(new UpdateBehaviourCommand(thread,
        () =>
        {
            if (reciever.isEmpty())
            {
                thread.Stop();
            }
            else
            {
                thread.HandleCommand();
            }
        }
        ));
    }
}
