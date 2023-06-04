namespace SpaceBattle.Lib;

using System.Collections;

public class InitPlayersPositionsStrategy : IStrategy
{
    public object Execute(params object[] argv)
    {
        var spaceshipsList = (List<IEnumerable<object>>)argv[0];

        return new InitPlayersPositionsCommand(spaceshipsList);
    }
}
