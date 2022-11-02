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
        this.n = n/gcd(n, m);
        this.m = m/gcd(n, m);
    }

    public override bool Equals(object? obj)
    {
        return obj is Angle angle &&
               n == angle.n &&
               m == angle.m;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(n, m);
    }

    public static Angle operator +(Angle a, Angle b)
    {
        int top = a.n * b.m + b.n * a.m;
        int bottom = a.m * b.m;
        return new Angle(top/gcd(top, bottom), bottom/gcd(top, bottom));
    }
    public static bool operator==(Angle a, Angle b)
    {
        if (a.n == b.n && a.m == b.m) return true;
        return false;
    }
    public static bool operator !=(Angle a, Angle b)
    {
        return !(a==b);
    }
    
}