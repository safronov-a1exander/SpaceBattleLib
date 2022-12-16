namespace SpaceBattle.Lib;

public interface IRotatable
{
    public Angle angle { get; set; }
    public Angle angleVelocity { get; }
}
