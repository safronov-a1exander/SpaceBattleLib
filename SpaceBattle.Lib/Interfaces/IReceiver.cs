namespace SpaceBattle.Lib;

public interface IReciver
{
    ICommand Receive();
    bool isEmpty();
}
