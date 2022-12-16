namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    IUObject UObject { get; }

    IList<int> Speed { get; }

    IQueue<ICommand> Queue { get; }
}
