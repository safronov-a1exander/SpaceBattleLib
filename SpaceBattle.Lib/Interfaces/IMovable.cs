namespace SpaceBattle.Lib;

public interface IMovable
{
    public Vector Speed { get; }
    public Vector Coords { get; set; }
}
