namespace SpaceBattle.Lib;
public class MoveCommand : ICommand
{
    IMovable objToMove;
    public MoveCommand(IMovable obj)
    {
        this.objToMove = obj;
    }
    public void Execute()
    {
        this.objToMove.Coords += this.objToMove.Speed;
    }
}
