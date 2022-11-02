namespace SpaceBattle.Lib;

interface IRotatable
{
    public List<Angle> CurrentAngle { get; set; }
    public List<Angle> AngleVelocity { get; }
}