namespace SpaceBattle.Lib;
using Hwdtech;
using System;
using System.Collections.Generic;

public class SerializeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string serializedGame = "";
        string divider = " | ";
        string colon = " : ";
        string semicolon = ";";

        string gameId = (string)args[0];

        Dictionary<string, object> gameObjects = IoC.Resolve<Dictionary<string, object>>("Game.Objects.GetAll", gameId);
        Queue<ICommand> gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.Get", gameId);
        TimeSpan timespan = IoC.Resolve<TimeSpan>("Game.Get.Timespan", gameId);

        foreach (KeyValuePair<string, object> entry in gameObjects)
        {
            serializedGame += $"{entry.Key}{colon}{IoC.Resolve<string>("Methods.ToString", entry.Value)}{semicolon}"; //TODO поменял название стратки
        }

        serializedGame += divider;

        foreach (ICommand cmd in gameQueue.ToArray())
        {
            serializedGame += IoC.Resolve<string>("SerializeCommand", cmd);
        }

        serializedGame += $"{divider}{timespan}";

        return serializedGame;
    }
}
