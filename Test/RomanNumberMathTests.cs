using App;
using FluentAssertions;

namespace Test;

[TestClass]
public class RomanNumberMathTests
{
    [TestMethod]
    public void GivenManyRomanNumbers_WhenCallingSum_ShouldReturnRomanNumberWithCorrectSum()
    {
        RomanNumber first = new(1);
        RomanNumber second = new(2);
        RomanNumber third = new(3);
        RomanNumber result = RomanNumberMath.Sum(first, second, third);
        result.Value.Should().Be(first.Value + second.Value + third.Value);
    }
}
