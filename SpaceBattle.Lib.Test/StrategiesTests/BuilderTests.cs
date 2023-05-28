namespace SpaceBattle.Lib.Test;

public class BuilderTests
{
    [Fact]
    public void PositiveBuilderTest()
    {
        string header = "namespace SpaceBattle.Lib;public class IMovableAdapter : IMovable";

        string openFigBracket = "{";
        string closingFigBracket = "}";

        string targetField = "Dictionary<string, object> target;";
        string constructor = "public IMovableAdapter(object _target){target = (Dictionary<string, object>) _target;}";

        string speedHeader = "public SpaceBattle.Lib.Vector Speed";
        string speedGetMethod = "get => (SpaceBattle.Lib.Vector) target[\"Speed\"];";

        string coordsHeader = "public SpaceBattle.Lib.Vector Coords";
        string coordsGetMethod = "get => (SpaceBattle.Lib.Vector) target[\"Coords\"];";
        string coordsSetMethod = "set => target[\"Coords\"] = value;";

        string expected = 
        header + 
        openFigBracket + 
        targetField + 
        constructor + 
        speedHeader + 
        openFigBracket + 
        speedGetMethod + 
        closingFigBracket + 
        coordsHeader + 
        openFigBracket + 
        coordsGetMethod + 
        coordsSetMethod + 
        closingFigBracket + 
        closingFigBracket;

        Type? IMovableType = Type.GetType("SpaceBattle.Lib.IMovable, SpaceBattle.Lib", true, true);

        var adapterString = new Builder();
        adapterString.Add(IMovableType!);
        var actual = adapterString.Build();

        Assert.Equal(expected, actual);
    }
}
