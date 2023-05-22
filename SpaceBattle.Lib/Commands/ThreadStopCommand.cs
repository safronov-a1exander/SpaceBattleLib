namespace SpaceBattle.Lib;

public class ThreadStopCommand : ICommand
{
    ServerThread stoppingThread;
    public ThreadStopCommand(ServerThread stoppingThread) => this.stoppingThread = stoppingThread;

    public void Execute()
    {
        stoppingThread.Stop();
    }
}
