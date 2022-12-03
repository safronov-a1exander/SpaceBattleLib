namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class RotateCommandTest
{
    [Fact]
    public void PosTestRotate()
    {
        var m = new Mock<IRotatable>();
        m.Setup(_m => _m.angle).Returns(new Angle(45, 1)).Verifiable();
        m.Setup(_m => _m.angleVelocity).Returns(new Angle(90, 1));

        var c = new RotateCommand(m.Object);
        c.Execute();

        m.VerifySet(_m => _m.angle = new Angle(135, 1));
    }

    [Fact]
    public void NegTestRotate_UnableToGetAngle()
    {
        var m = new Mock<IRotatable>();
        m.SetupGet(_m => _m.angle).Throws<Exception>();
        m.SetupGet(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void NegTestRotate_UnableToGetVelocity()
    {
        var m = new Mock<IRotatable>();
        m.SetupGet(_m => _m.angle).Returns(new Angle(45, 1)).Verifiable();
        m.SetupGet(_m => _m.angleVelocity).Throws<Exception>();


        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<Exception>();
    }

    [Fact]
    public void NegTestRotate_UnableToSetAngle()
    {
        var m = new Mock<IRotatable>();
        m.SetupProperty(_m => _m.angle, new Angle(45, 1));
        m.SetupGet(_m => _m.angleVelocity).Returns(new Angle(90, 1));
        m.SetupSet(_m => _m.angle = It.IsAny<Angle>()).Throws<Exception>();
        
        var c = new RotateCommand(m.Object);
        var act = () => c.Execute();

        act.Should().Throw<Exception>();
    }
}
