using Hwdtech;
using System.Collections.Generic;
namespace SpaceBattle.Lib;

public class InitiateThreadDependenciesStrategy : IStrategy
{

    public object Execute(params object[] args)
    {
        new Hwdtech.Ioc.InitScopeBasedIoCImplementationCommand().Execute();

        var scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        Dictionary<string, Dictionary<string, IUObject>> GamesObjects = new();

        Dictionary<string, IUObject> game = new();

        Queue<ICommand> _queue = new();

        game.Add("obj123", new UObject());

        GamesObjects.Add((string)args[0], game);
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CurrentGameId", (object[] args) =>
        {
            return (string)args[0];
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Get Object by ids", (object[] args) =>
        {
            string GameID = (string)args[0];

            string ObjectID = (string)args[1];

            IUObject obj = GamesObjects[GameID][ObjectID];

            return obj;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Message deserialize", (object[] args) =>
        {
            ICommand cmd = (ICommand)args[0];

            return cmd;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Command", (object[] args) =>
        {
            ICommand cmd = (ICommand)args[0];

            IoC.Resolve<ISender>("GetInternalSenderByThreadId", IoC.Resolve<string>("CurrentGameId")).Send(cmd);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Send Order", (object[] args) =>
        {
            ICommand cmd = (ICommand)args[0];

            IoC.Resolve<ISender>("GetOrderSenderByThreadId", Hwdtech.IoC.Resolve<string>("CurrentGameId")).Send(cmd);
        }).Execute();
        
        return game;
    }
}
