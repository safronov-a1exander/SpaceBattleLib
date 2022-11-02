namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class UnitTest1
{
    [Fact]
    public void PosTestMove()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.Setup(_m => _m.Speed).Returns(new Vector(-7, 3)).Verifiable();
        var c = new MoveCommand(m.Object);
        c.Execute();
        m.VerifySet(_m => _m.Coords = new Vector(5, 8));
    }
    [Fact]
    public void NegTestMove_UnableToGetCoords()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Coords).Throws<NullReferenceException>();
        m.Setup(_m => _m.Speed).Returns(new Vector(-7, 3)).Verifiable();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<NullReferenceException>();
    }
    [Fact]
    public void NegTestMove_UnableToGetSpeed()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.Setup(_m => _m.Speed).Throws<NullReferenceException>();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<NullReferenceException>();
    }
    [Fact]
    public void NegTestMove_UnableToAdd()
    {
        var m = new Mock<IMovable>();
        m.Setup(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.Setup(_m => _m.Speed).Returns(new Vector(-7, 3, 1)).Verifiable();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<ArgumentException>();
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
