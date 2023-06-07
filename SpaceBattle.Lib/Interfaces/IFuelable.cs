namespace SpaceBattle.Lib;

public interface IFuelable
{
    int fuelLevel { get; set; }

    int burnSpeed { get; }
}
