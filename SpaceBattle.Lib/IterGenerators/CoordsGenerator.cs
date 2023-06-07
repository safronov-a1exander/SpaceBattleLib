namespace SpaceBattle.Lib;

using System.Collections;

public class CoordsGenerator : IEnumerator<Vector>
{
    int count;
    int initialCount;

    Vector initialPos;
    Vector value;
    Vector step;

    public CoordsGenerator(int count, Vector start, Vector step)
    {
        this.initialPos = this.value = start;
        this.initialCount = this.count = count;
        this.step = step;
    }

    public Vector Current => value;

    object IEnumerator.Current => this.Current;

    public void Dispose() { }

    public bool MoveNext()
    {
        if (count <= 0) return false;
        this.value = this.value + this.step;

        --count;
        return true;
    }

    public void Reset()
    {
        this.count = this.initialCount;
        this.value = this.initialPos;
    }
}
