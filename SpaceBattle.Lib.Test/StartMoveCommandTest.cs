namespace SpaceBattle.Lib.Test;
using Moq;
using FluentAssertions;

public class StartMoveCommandTest
{
    [Fact]
    public void PosTestStartMoveCommand()
    {
        var m = new Mock<IMovable>();
        m.SetupGet(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.SetupGet(_m => _m.Speed).Returns(new Vector(-7, 3)).Verifiable();
        var c = new MoveCommand(m.Object);
        c.Execute();
        m.VerifySet(_m => _m.Coords = new Vector(5, 8));
    }
    [Fact]
    public void NegTestStartMoveCommand_UnableToGetUObject()
    {
        var m = new Mock<IMoveCommandStartable>();
        m.SetupGet(_m => _m.UObject).Throws<NullReferenceException>();
        var c = new StartMoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<NullReferenceException>();
    }
    [Fact]
    public void NegTestStartMoveCommand_UnableToGetSpeed()
    {
        var m = new Mock<IMovable>();
        m.SetupGet(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.SetupGet(_m => _m.Speed).Throws<NullReferenceException>();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<NullReferenceException>();
    }
    [Fact]
    public void NegTestMove_UnableToAdd()
    {
        var m = new Mock<IMovable>();
        m.SetupGet(_m => _m.Coords).Returns(new Vector(12, 5)).Verifiable();
        m.SetupGet(_m => _m.Speed).Returns(new Vector(-7, 3, 1)).Verifiable();
        var c = new MoveCommand(m.Object);
        var act = () => c.Execute();
        act.Should().Throw<ArgumentException>();
    }
}
