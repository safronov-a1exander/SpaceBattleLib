namespace SpaceBattle.Lib;

public interface IQueue<T>
{
    void Push(T elem);

    T Pop();
}
