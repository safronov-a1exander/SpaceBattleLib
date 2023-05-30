namespace SpaceBattle.Lib.Test;

public class BuilderTests
{
    [Fact]
    public void PositiveBuilderTest()
    {
        string expected = "namespace SpaceBattle.Lib;public class IMovableAdapter : IMovable{Dictionary<string, object> target;public IMovableAdapter(object _target){target = (Dictionary<string, object>) _target;}public SpaceBattle.Lib.Vector Speed{get => (SpaceBattle.Lib.Vector) target[\"Speed\"];}public SpaceBattle.Lib.Vector Coords{get => (SpaceBattle.Lib.Vector) target[\"Coords\"];set => target[\"Coords\"] = value;}}";

        Type? IMovableType = Type.GetType("SpaceBattle.Lib.IMovable, SpaceBattle.Lib", true, true);

        var adapterString = new Builder();
        adapterString.Add(IMovableType!);
        var actual = adapterString.Build();

        Assert.Equal(expected, actual);
    }
}
