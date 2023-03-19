namespace SpaceBattle.Lib;

class ThreadStopCommand : ICommand
{
    MyThread stoppingThread;
    public ThreadStopCommand(MyThread stoppingThread) => this.stoppingThread = stoppingThread;

    public void Execute()
    {
        if (Thread.CurrentThread == stoppingThread)
        {
            stoppingThread.Stop();
        }
        else
        {
            throw new Exception();
        }
    }
}
