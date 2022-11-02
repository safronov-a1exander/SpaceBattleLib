namespace SpaceBattle.Lib;

interface IRotatable
{
    public Angle CurrentAngle { get; set; }
    public Angle AngleVelocity { get; }
}