namespace SpaceBattle.Lib.Test;
using FluentAssertions;

public class VectorTest
{
    [Fact]
    public void PosTestVec_Create()
    {
        Vector t = new Vector(1, 1);
        Assert.NotNull(t);
    }

    [Fact]
    public void NegTestVec_Create()
    {
        Assert.Throws<ArgumentException>(() => new Vector());
    }

    [Fact]
    public void PosTestVec_ToStr()
    {
        var obj = new Vector(12, 24);
        Assert.Equal("Vector (12, 24)", obj.ToString());
    }

    [Fact]
    public void PosTestVec_Ind()
    {
        var obj = new Vector(1, 2);
        obj[1] += obj[0];
        Assert.Equal(new Vector(1, 3), obj);
    }

    [Fact]
    public void NegTestVec_Ind()
    {
        var obj = new Vector(1, 2);
        var act = () => obj[1] += obj[2];
        act.Should().Throw<IndexOutOfRangeException>();
    }

    [Fact]
    public void PosTestVec_Eq()
    {
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 == obj2);
    }

    [Fact]
    public void NegTestVec_Eq1()
    {
        var obj1 = new Vector(1, 3);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 != obj2);
    }

    [Fact]
    public void NegTestVec_Eq2()
    {
        var obj1 = new Vector(1, 3, 4);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 == obj2);
    }

    [Fact]
    public void NegTestVec_Eq3()
    {
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 != obj2);
    }

    [Fact]
    public void PosTestVec_Hash()
    {
        var obj1 = new Vector(1, 3);
        Assert.IsType<int>(obj1.GetHashCode());
    }

    [Fact]
    public void PosTestVec_Less1()
    {
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 3);
        Assert.True(obj1 < obj2);
    }

    [Fact]
    public void PosTestVec_Less2()
    {
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2, 3);
        Assert.True(obj1 < obj2);
    }

    [Fact]
    public void NegTestVec_Less1()
    {
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 < obj2);
    }

    [Fact]
    public void NegTestVec_Less2()
    {
        var obj1 = new Vector(1, 2, 3);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 < obj2);
    }

    [Fact]
    public void NegTestVec_Less3()
    {
        var obj1 = new Vector(2, 2);
        var obj2 = new Vector(1, 2);
        Assert.False(obj1 < obj2);
    }

    [Fact]
    public void PosTestVec_More1()
    {
        var obj1 = new Vector(1, 3);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 > obj2);
    }

    [Fact]
    public void PosTestVec_More2()
    {
        var obj1 = new Vector(1, 2, 3);
        var obj2 = new Vector(1, 2);
        Assert.True(obj1 > obj2);
    }

    [Fact]
    public void PosTestVec_Substr()
    {
        var obj1 = new Vector(1, 2, 3);
        var obj2 = new Vector(1, 2, 1);
        var obj3 = obj1 - obj2;
        Assert.Equal(new Vector(0, 0, 1), obj3);
    }

    [Fact]
    public void NegTestVec_Substr()
    {
        var obj1 = new Vector(1, 2);
        var obj2 = new Vector(1, 2, 1);
        var act = () => obj1 -= obj2;
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void PosTestVec_Mult()
    {
        var obj1 = new Vector(1, 2, 3);
        var obj2 = 3 * obj1;
        Assert.Equal(new Vector(3, 6, 9), obj2);
    }
}
