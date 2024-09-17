using App;
using FluentAssertions;

namespace Test;

[TestClass]
public class PublicNumberExtensionTests
{
    [TestMethod]
    public void GivenManyRomanNumbers_WhenCallingSum_ShouldReturnRomanNumberWithCorrectSum()
    {
        RomanNumber first = new(1);
        RomanNumber second = new(2);
        RomanNumber third = new(3);
        RomanNumber result = first.Sum(second, third);
        result.Value.Should().Be(first.Value + second.Value + third.Value);
    }
}
