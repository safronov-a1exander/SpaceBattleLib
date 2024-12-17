using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class GameTransitionTests
{
    static object scope;
    static Queue<ICommand> queue;
    static Queue<ICommand> deserializedQueue;
    static Dictionary<string, object> gameObjects;
    static Dictionary<string, object> deserializedGameObjects;
    static TimeSpan ts;
    static TimeSpan dts;
    static GameTransitionTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        scope = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"));

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        deserializedGameObjects = new();

        deserializedQueue = new();

        dts = new();

        gameObjects = new();

        queue = new();

        ts = new(0, 0, 0, 0, 100);

        Mock<ICommand> mcmd = new();

        mcmd.Setup(c => c.Execute()).Callback(() => { });

        ICommand cmd = mcmd.Object;

        IUObject _obj = new UObject();

        gameObjects.Add("object75", _obj);

        ICommand command = new ActionCommand(() => { });

        queue.Enqueue(command);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeValue", (object[] args) =>
        {
            string serializedString = (string)args[0];
            return (object)_obj;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeCommand", (object[] args) =>
        {
            string serializedCommand = (string)args[0];
            return (object)command;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "DeserializeTimespan", (object[] args) =>
        {
            string serializedTimespan = (string)args[0];
            return (object)ts;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateNewGameCommand", (object[] args) =>
        {
            deserializedGameObjects = (Dictionary<string, object>)args[0];
            deserializedQueue = (Queue<ICommand>)args[1];
            dts = (TimeSpan)args[2];
            return (object)new ActionCommand(() => { });
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Objects.GetAll", (object[] args) =>
        {
            return gameObjects;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Queue.Get", (object[] args) =>
        {
            return queue;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Timespan", (object[] args) =>
        {
            return (object)ts;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Current.Queue", (object[] args) =>
        {
            return queue;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetProps", (object[] args) =>
        {
            Dictionary<string, object> dict = new() { { "key", "string value" } };
            return dict;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.GetAction", (object[] args) => "{}").Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Methods.ToString", (object[] args) =>
            args[0].ToString() + IoC.Resolve<string>("StringifyObjectProps", args[0])
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "StringifyObjectProps", (object[] args) => "{key=string value, key2=int 123}").Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "SerializeCommand", (object[] args) =>
       {
           ICommand cmd = (ICommand)args[0];

           string return_string = "Command ";

           if (cmd is MoveCommand)
           {
               return_string += "type=move, ";
               foreach (var prop in IoC.Resolve<Dictionary<string, object>>("Command.GetProps", cmd))
               {
                   return_string += prop.Key + " : " + prop.Value.ToString();
               }
           }
           if (cmd is ActionCommand)
           {
               return_string += "type=action, ";
               return_string += "action=" + IoC.Resolve<string>("Command.GetAction", cmd);
           }

           return return_string;
       }).Execute();
    }

    [Fact]
    public void SerializationTest()
    {
        string serializedGame = (string)new SerializeStrategy().Execute("1");

        Assert.Equal("object75 : SpaceBattle.Lib.UObject{key=string value, key2=int 123}; | Command type=action, action={} | 00:00:00.1000000", serializedGame);
    }

    [Fact]
    public void DeserializationTest()
    {
        string serializedGame = (string)new SerializeStrategy().Execute("1");
        _ = (ICommand)new DeserializeStrategy().Execute(serializedGame);

        Assert.Equal(queue, deserializedQueue);
        Assert.Equal(gameObjects, deserializedGameObjects);
        Assert.Equal(ts, dts);
    }
}
