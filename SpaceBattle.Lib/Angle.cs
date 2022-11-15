namespace SpaceBattle.Lib;

public class Angle {
    public int n;
    public int m;

    private static int gcd(int x, int y)
    {
        return Math.Abs(y) == 0 ? Math.Abs(x) : gcd(Math.Abs(y), Math.Abs(x) % Math.Abs(y));
    } 

    public Angle(int n, int m)
    {
        if (m == 0) throw new DivideByZeroException();
        if (n >=0 && m < 0 || n <= 0 && m < 0)
        {
            n *= -1;
            m *= -1;
        }
        int gcd_temp = gcd(n, m);
        this.n = n/gcd_temp;
        this.m = m/gcd_temp;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(n, m);
    }

    public static Angle operator +(Angle a, Angle b)
    {
        int top = a.n * b.m + b.n * a.m;
        int bottom = a.m * b.m;
        int tbgcd = gcd(top, bottom);
        return new Angle(top/tbgcd, bottom/tbgcd);
    }
    public static bool operator==(Angle a, Angle b)
    {
        return (a.n == b.n && a.m == b.m);
    }
    public static bool operator !=(Angle a, Angle b)
    {
        return !(a==b);
    }
    public override bool Equals(object? obj) => obj is Angle a && n == a.n && m == a.m;
}
