namespace SpaceBattle.Lib;

public static class CoordsInitializer
{
    public static IEnumerator<Vector> GetEnumerator()
    {
        for (int i = 0; i < 6; i++)
        {
            if (i < 3)
            yield return new Vector(0, i);
            else yield return new Vector(5, i - 3);
        }
    }
}
