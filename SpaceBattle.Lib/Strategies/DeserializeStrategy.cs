namespace SpaceBattle.Lib;
using Hwdtech;
using System;
using System.Collections.Generic;

public class DeserializeStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        string divider = " | ";
        string colon = " : ";
        string semicolon = ";";
        string comma = ",";

        string serializedGame = (string)args[0];
        string[] serializedData = serializedGame.Split(divider);

        Dictionary<string, object> gameObjects = new();
        Queue<ICommand> gameQueue = new();
        foreach (string propertyData in serializedData[0].Split(semicolon))
        {
            if (propertyData.Contains(colon))
            {
                string key = propertyData.Split(colon)[0];
                string stringValue = propertyData.Split(colon)[1];

                object objectValue = IoC.Resolve<object>("DeserializeValue", stringValue);

                gameObjects[key] = objectValue;
            }
        }

        foreach (string commandData in serializedData[1].Split(comma))
        {
            if (commandData.Contains("type"))
            {
                ICommand deserializedCommand = IoC.Resolve<ICommand>("DeserializeCommand", commandData);

                gameQueue.Enqueue(deserializedCommand);
            }
        }

        TimeSpan timespan = IoC.Resolve<TimeSpan>("DeserializeTimespan", serializedData[2]);

        ICommand newGameCommand = IoC.Resolve<ICommand>("CreateNewGameCommand", gameObjects, gameQueue, timespan);

        return newGameCommand;
    }
}
