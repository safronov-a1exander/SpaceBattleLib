namespace SpaceBattle.Lib;
using Hwdtech;

public class InitPlayersCoordsCommand : ICommand
{
    List<IEnumerable<object>> spaceshipsList;

    public InitPlayersCoordsCommand(List<IEnumerable<object>> spaceshipsList)
    {
        this.spaceshipsList = spaceshipsList;
    }

    public void Execute()
    {
        foreach(IEnumerable<object> enumer in spaceshipsList)
        {
            IoC.Resolve<ICommand>("Game.Objects.Commands.InitCoords", enumer).Execute();
        }
    }
}
