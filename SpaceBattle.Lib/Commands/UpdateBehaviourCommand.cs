namespace SpaceBattle.Lib;

class UpdateBehaviourCommand : ICommand
{
    ActionCommand behaviour;
    ServerThread thread;

    public UpdateBehaviourCommand(ServerThread thread, ActionCommand newBehaviour)
    {
        this.behaviour = newBehaviour;
        this.thread = thread;
    }
    public void Execute()
    {
        thread.UpdateBehaviour(behaviour);
    }
}
