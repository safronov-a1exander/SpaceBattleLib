namespace SpaceBattle.Lib;

using System.Collections;

public class InitPlayersCoordsStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var spaceshipsList = (List<IEnumerable<object>>)args[0];

        return new InitPlayersCoordsCommand(spaceshipsList);
    }
}
