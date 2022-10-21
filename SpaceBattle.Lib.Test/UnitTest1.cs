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
    public void PosTest_CreateVec(){
        Vector t = new Vector(1, 1);
        Assert.NotNull(t);
    }

    [Fact]
    public void NegTest_CreateVec(){
        Assert.Throws<ArgumentException>(() => new Vector());
    }
    
    [Fact]
    public void Test_VectorToStr(){
        var obj = new Vector(12, 24);
        Assert.Equal("Vector (12, 24)", obj.ToString());
    }

    [Fact]
    public void PosTest_VectorInd(){
        var obj = new Vector(1, 2);
        obj[1] += obj[0];
        Assert.Equal(new Vector(1, 3), obj);
    }

    [Fact]
    public void NegTest_VectorInd(){
        var obj = new Vector(1, 2);
        var act = () => obj[1] += obj[2];
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void PosTest_VectorEq(){
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void NegTest_VectorEq1(){
        var obj1 = new Vector(1, 3);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 != obj2);
    }

    [Fact]
    public void NegTest_VectorEq2(){
        var obj1 = new Vector(1, 3, 4);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 == obj2);
    }

    [Fact]
    public void NegTest_VectorEq3(){
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 != obj2);
    }

    [Fact]
    public void Test_VectorHash(){
        var obj1 = new Vector(1, 3);
        Assert.IsType<int>(obj1.GetHashCode());
    }

    [Fact]
    public void PosTest_VectorLess1(){
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 3);
        Assert.True(obj1 < obj2);
    }

    [Fact]
    public void PosTest_VectorLess2(){
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2, 3);
        Assert.True(obj1 < obj2);
    }

    [Fact]
    public void NegTest_VectorLess1(){
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 < obj2);
    }

    [Fact]
    public void NegTest_VectorLess2(){
        var obj1 = new Vector(1, 2, 3);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 < obj2);
    }    

    [Fact]
    public void NegTest_VectorLess3(){
        var obj1 = new Vector(2, 2);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 < obj2);
    }

    [Fact]
    public void PosTest_VectorMore1(){
        var obj1 = new Vector(1, 3);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 > obj2);
    }

    [Fact]
    public void PosTest_VectorMore2(){
        var obj1 = new Vector(1, 2, 3);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 > obj2);
    }

    [Fact]
    public void PosTest_VectorSubstr(){
        var obj1 = new Vector(1, 2, 3);
        var obj2 = new Vector(1, 2, 1);
        var obj3 = obj1 - obj2;
        Assert.Equal(new Vector(0, 0, 1), obj3);
    }

    [Fact]
    public void NegTest_VectorSubstr1(){
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2, 1);
        var act = () => obj1 -= obj2;
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void PosTest_VectorMult(){
        var obj1 = new Vector(1, 2, 3);
        var obj2 = 3 * obj1;
        Assert.Equal(new Vector(3, 6, 9), obj2);
    }
}