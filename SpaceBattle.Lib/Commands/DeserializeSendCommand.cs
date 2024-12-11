namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections.Generic;

public class DeserializeSendCommand: ICommand{

    private Dictionary<string, object> props;
    private string command;
    private string id;

    public DeserializeSendCommand(string id, string command, Dictionary<string, object> properties)
    {
        this.props = properties;
        this.command = command;
        this.id = id;
    }
    public void Execute()
    {
        ICommand cmd = IoC.Resolve<ICommand>("Create" + command + "FromMessage", id, props);

        IoC.Resolve<ICommand>("Send Command", cmd).Execute();
    }
}
