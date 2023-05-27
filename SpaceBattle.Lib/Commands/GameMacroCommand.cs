using System.Diagnostics;
using Hwdtech;
namespace SpaceBattle.Lib;

public class GameMacroCommand : ICommand
{
    string id;
    object scope;
    GameCommand gc;
    public GameMacroCommand(string id, object scope)
    {
        this.id = id;
        this.scope = scope;
        gc = new GameCommand(id);
    }

    public void Execute()
    {
        var previous_scope = IoC.Resolve<object>("Scopes.Current");
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", this.scope).Execute();

        this.gc.Execute();

        var sender = IoC.Resolve<string>("Storage.GetThreadByGameID", this.id);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", sender, this).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", previous_scope).Execute();
    }

}
