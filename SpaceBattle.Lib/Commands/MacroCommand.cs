namespace SpaceBattle.Lib;

public class MacroCommand : ICommand
{
    private IList<ICommand> listOfCommands;

    public MacroCommand(IList<ICommand> listOfCommands)
    {
        this.listOfCommands = listOfCommands;
    }

    public void Execute()
    {
        foreach (ICommand cmd in listOfCommands)
        {
            cmd.Execute();
        }
    }
}
