namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class RotateUnitTest2 {
    [Fact]
    public void PosTest_CreateAngle()
    {
        Assert.IsType<Angle>(new Angle(60, 1));
    }

    [Fact]
    public void RotateCommandSetAngleExceptionNegative_CreateAngle()
    {
        Assert.Throws<DivideByZeroException>(() => new Angle(45, 0));
    }

    [Fact]
    public void PosTest_AngleEq()
    {
        Angle a = new Angle(45, -1);
        Angle b = new Angle(-135, 3);
        Assert.True(a == b);
    }

    [Fact]
    public void Test_AngleEqMethod()
    {
        Angle a = new Angle(-30, -1);
        int b = 1;
        Assert.False(a.Equals(b));
    }
    
    [Fact]
    public void RotateCommandSetAngleExceptionNegative_AngleEq()
    {
        Angle a = new Angle(45, -1);
        Angle b = new Angle(-135, 45);
        Assert.False(a == b);
    }
    
    [Fact]
    public void Test_AngleNotEq()
    {
        Angle a = new Angle(45, -1);
        Angle b = new Angle(-135, 45);
        Assert.True(a != b);
    }
    
    [Fact]
    public void Test_AngleAdd()
    {
        Angle a = new Angle(45, 1);
        Angle b = new Angle(90, 1);
        Assert.Equal(new Angle(135, 1), a + b);
    }
    
    [Fact]
    public void Test_AngleHash()
    {
        Angle a = new Angle(-45, 1);
        Angle b = new Angle(135, -3);
        Assert.True(a.GetHashCode() == b.GetHashCode());
    }
}