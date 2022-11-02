namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class UnitTest1
{
    [Fact]
    public void PosTest_Rotate()
    {
        var m = new Mock<IRotatable>();
        m.Setup(_m => _m.angle).Returns(new Angle(45, 1)).Verifiable();
        m.Setup(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        
        var c = new RotateCommand(m.Object);
        c.Execute();

        m.VerifySet(_m => _m.angle = new Angle(135, 1));
    }

    [Fact]
    public void NegTest_Rotate1()
    {
        var m = new Mock<IRotatable>();
        m.Setup(_m => _m.angle).Throws<NullReferenceException>();
        m.Setup(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void NegTest_Rotate2()
    {
        var m = new Mock<IRotatable>();
        m.Setup(_m => _m.angle).Returns(new Angle(45, 1)).Verifiable();
        m.Setup(_m => _m.angleVelocity).Throws<NullReferenceException>();
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void NegTest_Rotate3()
    {
        var m = new Mock<IRotatable>();
        m.SetupProperty(_m => _m.angle, new Angle(45, 1));
        m.Setup(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        m.SetupGet(_m => _m.angle).Throws<ArithmeticException>();
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<ArithmeticException>();
    }
}

public class UnitTest2 {
    [Fact]
    public void PosTest_CreateAngle()
    {
        Assert.IsType<Angle>(new Angle(60, 1));
    }

    [Fact]
    public void NegTest_CreateAngle()
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
    public void NegTest_AngleEq()
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
