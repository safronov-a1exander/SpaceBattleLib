namespace SpaceBattle.Lib;

public class SendCommand : ICommand
{
    ISender sender;
    ICommand command;
    public SendCommand(ISender sndr, ICommand cmd)
    {
        this.sender = sndr;
        this.command = cmd;
    }
    public void Execute()
    {
        sender.Send(command);
    }
}
