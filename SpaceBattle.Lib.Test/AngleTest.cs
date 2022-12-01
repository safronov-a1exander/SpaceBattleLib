namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class AngleTest {
    [Fact]
    public void PosTestAngle_Create()
    {
        Assert.IsType<Angle>(new Angle(60, 1));
    }

    [Fact]
    public void NegTestAngle_CreateDivideByZero()
    {
        Assert.Throws<DivideByZeroException>(() => new Angle(45, 0));
    }

    [Fact]
    public void PosTestAngle_Equal()
    {
        Angle a = new Angle(45, -1);
        Angle b = new Angle(-135, 3);
        Assert.True(a == b);
    }

    [Fact]
    public void NegTestAngle_CheckEquals()
    {
        Angle a = new Angle(-30, -1);
        int b = 1;
        Assert.False(a.Equals(b));
    }
    
    [Fact]
    public void NegTestAngle_Equal()
    {
        Angle a = new Angle(45, -1);
        Angle b = new Angle(-135, 45);
        Assert.False(a == b);
    }
    
    [Fact]
    public void PosTestAngle_NotEqual()
    {
        Angle a = new Angle(45, -1);
        Angle b = new Angle(-135, 45);
        Assert.True(a != b);
    }
    
    [Fact]
    public void PosTestAngle_Sum()
    {
        Angle a = new Angle(45, 1);
        Angle b = new Angle(90, 1);
        Assert.Equal(new Angle(135, 1), a + b);
    }
    
    [Fact]
    public void PosTestAngle_HashCode()
    {
        Angle a = new Angle(-45, 1);
        Angle b = new Angle(135, -3);
        Assert.True(a.GetHashCode() == b.GetHashCode());
    }
}
