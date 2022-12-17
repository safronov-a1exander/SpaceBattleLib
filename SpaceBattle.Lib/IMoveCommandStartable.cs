namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    IUObject UObject { get; }

    Vector Speed { get; }

    IQueue<ICommand> Queue { get; }
}
