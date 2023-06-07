namespace SpaceBattle.Lib;
using Hwdtech;
using System.Collections;

public class FuelGenerator : IEnumerator<int>
{
    IList<object> objs;

    IEnumerator<object> currentObjEnumerator;


    public FuelGenerator(IList<object> objs)
    {
        this.objs = objs;
        this.currentObjEnumerator = objs.GetEnumerator();
    }

    public int Current => IoC.Resolve<int>("Game.Objects.Properties.Get", "Fuel", currentObjEnumerator.Current);

    object IEnumerator.Current => Current;

    public void Dispose() { }

    public bool MoveNext()
    {
        return this.currentObjEnumerator.MoveNext();
    }

    public void Reset()
    {
        this.currentObjEnumerator = this.objs.GetEnumerator();
    }
}
