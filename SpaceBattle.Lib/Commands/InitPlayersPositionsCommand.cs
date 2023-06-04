namespace SpaceBattle.Lib;
using Hwdtech;

public class InitPlayersPositionsCommand : ICommand
{
    List<IEnumerable<object>> spaceshipsList;

    public InitPlayersPositionsCommand(List<IEnumerable<object>> spaceshipsList)
    {
        this.spaceshipsList = spaceshipsList;
    }

    public void Execute()
    {
        foreach(IEnumerable<object> enumer in spaceshipsList)
        {
            IoC.Resolve<ICommand>("Game.Objects.Commands.InitPosition", enumer).Execute();
        }
    }
}
