namespace SpaceBattle.Lib;

public class RotateCommand : ICommand
{
    IRotatable objToRotate;
    public RotateCommand(IRotatable obj)
    {
        objToRotate = obj;
    }
    public void Execute()
    {
        this.objToRotate.angle += this.objToRotate.angleVelocity;
    }
}
