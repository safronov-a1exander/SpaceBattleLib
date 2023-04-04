namespace SpaceBattle.Lib;

class ThreadStopCommand : ICommand
{
    ServerThread stoppingThread;
    public ThreadStopCommand(ServerThread stoppingThread) => this.stoppingThread = stoppingThread;

    public void Execute()
    {
        if (Thread.CurrentThread == stoppingThread.thread)
        {
            stoppingThread.Stop();
        }
        else
        {
            throw new Exception();
        }
    }
}
