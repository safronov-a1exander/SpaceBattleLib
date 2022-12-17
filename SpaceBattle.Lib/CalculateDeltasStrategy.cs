namespace SpaceBattle.Lib;
using Hwdtech;

public class CalculateDeltas : IStrategy
{
    public object Execute(params object[] args)
    {
        var first = (IUObject)args[0];
        var second = (IUObject)args[1];

        var firstPos = IoC.Resolve<Vector>("UObject.GetProperty", first, "Coords");
        var secondPos = IoC.Resolve<Vector>("UObject.GetProperty", second, "Coords");
        var firstSpeed = IoC.Resolve<Vector>("UObject.GetProperty", first, "Speed");
        var secondSpeed = IoC.Resolve<Vector>("UObject.GetProperty", second, "Speed");

        return new List<int> { firstPos[0] - secondPos[0], firstPos[1] - secondPos[1], firstSpeed[0] - secondSpeed[0], firstSpeed[1] - secondSpeed[1] };
    }
}
