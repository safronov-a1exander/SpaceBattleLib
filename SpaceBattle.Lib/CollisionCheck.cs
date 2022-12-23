namespace SpaceBattle.Lib;
using Hwdtech;

public class CollisionCheck : ICommand
{
    private IUObject patient1, patient2;
    public CollisionCheck(IUObject _patient1, IUObject _patient2)
    {
        this.patient1 = _patient1;
        this.patient2 = _patient2;
    }

    public void Execute()
    {
        var st = IoC.Resolve<IDictionary<int, object>>("Get.SolutionTree");
        var deltas = IoC.Resolve<List<int>>("Operations.CalculateDeltas", this.patient1, this.patient2);
        var helper = st;
        deltas.ForEach(delta => helper = (IDictionary<int, object>)helper[delta]);
        if (helper.Keys.First() == 1)
        {
            IoC.Resolve<ICommand>("Event.Collision", this.patient1, this.patient2).Execute();
        }
    }
}
