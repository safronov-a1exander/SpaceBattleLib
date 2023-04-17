namespace SpaceBattle.Lib;

class ThreadStopCommand : ICommand
{
    ServerThread stoppingThread;
    public ThreadStopCommand(ServerThread stoppingThread) => this.stoppingThread = stoppingThread;

    public void Execute()
    {
        stoppingThread.Stop();
    }
}
