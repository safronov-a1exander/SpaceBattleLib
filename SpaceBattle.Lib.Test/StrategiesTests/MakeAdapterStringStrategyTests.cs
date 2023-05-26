namespace SpaceBattle.Lib.Test;

public class MakeAdapterStringStrategyTests
{
    [Fact]
    public void PositiveMakeAdapterStringStrategyTest()
    {
        string header = "namespace SpaceBattle.Lib;public class IMovableAdapter : IMovable";

        string openFigBracket = "{";
        string closingFigBracket = "}";

        string targetField = "IUObject target;";
        string constructor = "public IMovableAdapter(object _target){target = (IUObject) _target;}";

        string speedHeader = "public SpaceBattle.Lib.Vector Speed";
        string speedGetMethod = "get => (SpaceBattle.Lib.Vector) target.getProperty(\"Speed\");";

        string coordsHeader = "public SpaceBattle.Lib.Vector Coords";
        string coordsGetMethod = "get => (SpaceBattle.Lib.Vector) target.getProperty(\"Coords\");";
        string coordsSetMethod = "set => target.setProperty(\"Coords\", value);";

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

        string adapterString = (string) new MakeAdapterStringStrategy(IMovableType!).Execute();

        Assert.Equal(expected, adapterString);
    }
}