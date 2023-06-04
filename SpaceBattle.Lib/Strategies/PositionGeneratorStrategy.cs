namespace SpaceBattle.Lib;

public class PositionGeneratorStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var iterable = (IEnumerable<object>)args[0];
        var start = (Vector)args[1];
        var step = (Vector)args[2];

        return new PositionGenerator(count: iterable.Count(), start: start, step: step);
    }
}
