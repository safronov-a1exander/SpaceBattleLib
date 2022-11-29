namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class RotateCommandUnitTest1
{
    [Fact]
    public void Pos_Rotate()
    {
        var m = new Mock<IRotatable>();
        m.SetupProperty(_m => _m.angle).Returns(new Angle(45, 1)).Verifiable();
        m.SetupGet(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        
        var c = new RotateCommand(m.Object);
        c.Execute();

        m.VerifySet(_m => _m.angle = new Angle(135, 1));
    }

    [Fact]
    public void Neg_RotateCommandSetAngleException1()
    {
        var m = new Mock<IRotatable>();
        m.SetupProperty(_m => _m.angle, new Angle(45, 1)).Throws<NullReferenceException>();
        m.SetupGet(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Neg_RotateCommandSetVelocityException2()
    {
        var m = new Mock<IRotatable>();
        m.SetupGet(_m => _m.angle).Returns(new Angle(45, 1)).Verifiable();
        m.Setup(_m => _m.angleVelocity).Throws<NullReferenceException>();
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<NullReferenceException>();
    }

    [Fact]
    public void Neg_RotateCommandArithmeticException3()
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
