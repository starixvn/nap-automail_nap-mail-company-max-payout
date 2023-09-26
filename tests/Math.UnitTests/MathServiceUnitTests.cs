namespace Math.UnitTests;

using Xunit;

public class MathServiceUnitTests
{
    [Fact]
    public void Add_Always_ReturnRightValue()
    {
        const int a = 1;
        const int b = 2;
        const int expected = 3;

        var mathService = new MathService();
        var actual = mathService.Add(a, b);

        Assert.Equal(expected, actual);
    }
}