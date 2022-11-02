namespace SpaceBattle.Lib;

public interface IRotatable
{
    public Angle CurrentAngle { get; set; }
    public Angle AngleVelocity { get; }
}