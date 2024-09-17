using App;
using FluentAssertions;
using System.Reflection;

namespace Test;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    public void GivenStringInput_ShouldCreateCorrectRomanNumber()
    {
        RomanNumber romanNumber = new("IX");
        romanNumber.Value.Should().Be(9);
    }

    [TestMethod]
    public void ValidateInputTest_HappyPath_NoExceptions()
    {
        Type rnType = typeof(RomanNumber);
        MethodInfo? methodInfo = rnType.GetMethod("ValidateInput", BindingFlags.NonPublic | BindingFlags.Static);

        var happyPath = () => methodInfo?.Invoke(null, ["IX"]);
        happyPath.Should().NotThrow();
    }

    [TestMethod]
    public void GetDigitTest()
    {
        Type rnType = typeof(RomanNumber);
        MethodInfo? methodInfo = rnType.GetMethod("GetDigitTest", BindingFlags.NonPublic | BindingFlags.Static);

        methodInfo?.Invoke(null, ["IX"]);
        var act = () => methodInfo?.Invoke(null, ["IX"]);
        act.Should().NotThrow<FormatException>();
    }

    [TestMethod]
    [DynamicData(nameof(ValidDigitPairTestCases))]
    [DataRow("CXXIII", 123)]
    [DataRow("CDXLIV", 444)]
    [DataRow("CMXCIX", 999)]
    [DataRow("MMMCCCXLIII", 3343)]
    public void GivenSomeNumber_WhenCallingToString_ShouldReturnRomanNumberString(string expected, int number)
    {
        string result = new RomanNumber(number).ToString();
        result.Should().Be(expected);
    }

    [TestMethod]
    [DataRow("IV", "VI", "X")]
    [DataRow("I", "I", "II")]
    [DataRow("CMXCIX", "CDXLIV", "MCDXLIII")]
    public void GivenTwoRomanNumberStrings_WhenCallingPlus_ShouldReturnCorrectRomanNumberString(string first, string second, string expected)
    {
        string result = RomanNumberFactory.Parse(first).Plus(second);
        result.Should().NotBeNull();
        result.Should().BeOfType<string>();
        result.Should().NotBeSameAs(first);
        result.Should().NotBeSameAs(second);
        result.Should().Be(expected);
    }

    
    private static IEnumerable<object[]> ValidDigitPairTestCases
    {
        get => [
            ["N", 0 ],
            ["I", 1 ],
            ["V", 5 ],
            ["X", 10 ],
            ["L", 50 ],
            ["C", 100 ],
            ["D", 500 ],
            ["M", 1000 ],
        ];
    }
}