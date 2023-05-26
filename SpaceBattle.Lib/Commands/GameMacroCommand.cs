using System.Diagnostics;
using Hwdtech;
namespace SpaceBattle.Lib;

public class GameMacroCommand : ICommand
{
    string id;
    GameCommand gc;
    public GameMacroCommand(string id)
    {
        this.id = id;
        gc = new GameCommand(id);
    }

    public void Execute()
    {
        var previous_scope = IoC.Resolve<object>("Scopes.Current");
        var scope = IoC.Resolve<object>("Storage.GetScopeByGameID", this.id);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        this.gc.Execute();

        var sender = IoC.Resolve<string>("Storage.GetThreadByGameID", this.id);
        IoC.Resolve<SpaceBattle.Lib.ICommand>("Send Command", sender, this).Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", previous_scope).Execute();
    }

}
