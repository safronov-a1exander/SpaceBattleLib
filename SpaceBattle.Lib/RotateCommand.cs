namespace SpaceBattle.Lib;

public class RotateCommand : ICommand
{
    IRotatable objToRotate;
    private RotateCommand(IRotatable obj)
    {
        objToRotate = obj;
    }
    public void Execute()
    {
        this.objToRotate.CurrentAngle += this.objToRotate.AngleVelocity;
    }
}