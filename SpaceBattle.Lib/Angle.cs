namespace SpaceBattle.Lib;

public class Angle
{
    public int numerator;
    public int denominator;
    private static int GCD(int firstnum, int secondnum)
    {
        if (secondnum == 0) return firstnum;
        return GCD(secondnum,firstnum%secondnum);
    }
    public Angle(int numerator, int denominator)
    {
        if (denominator == 0) throw new DivideByZeroException();
        if (denominator < 0)
        {
            numerator *= -1;
            denominator *= -1;
        }
        this.numerator /= GCD(numerator,denominator);
        this.denominator /=GCD(numerator,denominator);
    }
    public override bool Equals(object? obj)
    {
        return obj is Angle angle &&
            numerator == angle.numerator &&
            denominator == angle.denominator;
    }
    public override int GetHashCode()
    {
        return (numerator.ToString()+denominator.ToString()).GetHashCode();
    }
    public static Angle operator + (Angle ang1, Angle ang2)
    {
        int numerator = ang1.numerator * ang2.denominator + ang2.numerator * ang1.denominator;
        int denominator = ang1.denominator * ang2.denominator;
        return new Angle(numerator, denominator);
    }
    public static bool operator == (Angle ang1, Angle ang2)
    {
        if (ang1.numerator == ang2.numerator && ang1.denominator == ang2.denominator) return true;
        return false;
    }
    public static bool operator != (Angle ang1, Angle ang2)
    {
        return !(ang1 == ang2);
    }
}
