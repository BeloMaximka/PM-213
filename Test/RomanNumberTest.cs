using App;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;

namespace Test;

[TestClass]
public class RomanNumberTest
{
    [TestMethod]
    [DynamicData(nameof(ValidNumberPairTestCases))]
    public void ParseTest(string romanNumber, int number)
    {
        RomanNumber rn = RomanNumber.Parse(romanNumber);
        Assert.IsNotNull(rn);
        Assert.AreEqual(
        number,
            rn.Value,
            $"{romanNumber} -> {number}"
        );
    }

    [TestMethod]
    [DataRow("W", 'W', 0)]
    [DataRow("Q", 'Q', 0)]
    [DataRow("s", 's', 0)]
    [DataRow("sX", 's', 0)]
    [DataRow("Xd", 'd', 1)]
    public void GivenIncorrectCharacter_WhenParsing_ShouldThrowFormatExceptionWithMessage(string input, char incorrectCharacter, int position)
    {
        var act = () => RomanNumber.Parse(input);
        act.Should().Throw<FormatException>().WithMessage($"Invalid character '{incorrectCharacter}' at index {position}");
    }

    [TestMethod]
    [DataRow("IM", 'I', 'M', 0)]
    [DataRow("IMX", 'I', 'M', 0)]
    [DataRow("XMD", 'X', 'M', 0)]
    [DataRow("LC", 'L', 'C', 0)]
    public void GivenIncorrectNumberFormat_WhenParsing_ShouldThrowFormatExceptionWithMessage(string input, char incorrectCharacter, char correctCharacter, int position)
    {
        var act = () => RomanNumber.Parse(input);
        act.Should().Throw<FormatException>().WithMessage($"Invalid order '{incorrectCharacter}' before '{correctCharacter}' at index {position} in \"{input}\"");
    }

    [TestMethod]
    [DataRow("IXC", 'X')]
    [DataRow("IIX", 'I')]
    [DataRow("VIX", 'I')]
    [DataRow("IIXC", 'X')]
    [DataRow("IIIX", 'I')]
    [DataRow("VIIX", 'I')]
    [DataRow("VIXC", 'X')]
    [DataRow("IVIX", 'I')]
    [DataRow("CVIIX", 'I')]
    [DataRow("IXCC", 'C')]
    [DataRow("IXCM", 'C')]
    [DataRow("IXXC", 'X')]
    public void GivenIncorrectNumberFormat_WhenParsing_ShouldThrowFormatException(string input, char characterAfterIncorrect)
    {
        var act = () => RomanNumber.Parse(input);
        act.Should().Throw<FormatException>().WithMessage($"Invalid sequence: more than 1 less digit before '{characterAfterIncorrect}'");
    }

    [TestMethod]
    public void ValidatePairsTest()
    {
        Type rnType = typeof(RomanNumber);
        MethodInfo? methodInfo = rnType.GetMethod("ValidatePairs", BindingFlags.NonPublic | BindingFlags.Static);

        methodInfo?.Invoke(null, ["IX"]);
        var act = () => methodInfo?.Invoke(null, ["IM"]);
        var exception = act.Should().Throw<TargetInvocationException>().And;
        exception.InnerException.Should().BeOfType<FormatException>();
    }

    [TestMethod]
    [DataRow("IXIV")]
    [DataRow("XCXL")]
    public void ValidateSubsTest(string input)
    {
        Type rnType = typeof(RomanNumber);
        MethodInfo? methodInfo = rnType.GetMethod("ValidateSubs", BindingFlags.NonPublic | BindingFlags.Static);

        var act = () => methodInfo?.Invoke(null, [input]);
        var exception = act.Should().Throw<TargetInvocationException>().And;
        exception.InnerException.Should().BeOfType<FormatException>();
    }

    [TestMethod]
    [DataRow("IXIX")]
    [DataRow("IXX")]
    [DataRow("IVIV")]
    [DataRow("XCC")]
    public void ValidateInputTest(string input)
    {
        Type rnType = typeof(RomanNumber);
        MethodInfo? methodInfo = rnType.GetMethod("ValidateInput", BindingFlags.NonPublic | BindingFlags.Static);

        var act = () => methodInfo?.Invoke(null, ["IXIX"]);

        var exception = act.Should().Throw<TargetInvocationException>().And;
        exception.InnerException.Should().BeOfType<FormatException>();
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
    public void DigitValueTest(string romanDigit, int expectedNumber)
    {

        RomanNumber.DigitValue(romanDigit).Should().Be(expectedNumber);
        var act = () => RomanNumber.DigitValue("IncorrectTestValue");
        var ex = act.Should().Throw<ArgumentException>().WithParameterName("digit").And;
        ex.Message.Should().NotBeNullOrEmpty();
        ex.Message.Should().Contain("'digit'");
        ex.Message.Should().Contain(nameof(RomanNumber));
        ex.Message.Should().Contain(nameof(RomanNumber.DigitValue));
        ex.Message.Should().Contain("IncorrectTestValue");
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
    public void GivenTwoIntegers_WhenCallingPlus_ShouldReturnRomanNumberWithCorrectSum()
    {
        RomanNumber first = new(1);
        RomanNumber second = new(2);
        RomanNumber result = first.Plus(second);
        result.Should().NotBeNull();
        result.Should().BeOfType<RomanNumber>();
        result.Should().NotBeSameAs(first);
        result.Should().NotBeSameAs(second);
        result.Value.Should().Be(first.Value + second.Value);
    }

    [TestMethod]
    [DataRow("IV", "VI", "X")]
    [DataRow("I", "I", "II")]
    [DataRow("CMXCIX", "CDXLIV", "MCDXLIII")]
    public void GivenTwoRomanNumberStrings_WhenCallingPlus_ShouldReturnCorrectRomanNumberString(string first, string second, string expected)
    {
        string result = RomanNumber.Parse(first).Plus(second);
        result.Should().NotBeNull();
        result.Should().BeOfType<string>();
        result.Should().NotBeSameAs(first);
        result.Should().NotBeSameAs(second);
        result.Should().Be(expected);
    }

    private static IEnumerable<object[]> ValidNumberPairTestCases
    {
        get => [
                ["N",    0],
                ["I",    1],
                ["II",   2],
                ["III",  3],
                ["IIII", 4],   // цим тестом ми дозволяємо неоптимальну форму числа
                ["IV",   4],
                ["VI",   6],
                ["VII",  7],
                ["VIII", 8],
                ["IX",   9],
                ["D",    500],
                ["M",    1000],
                ["CM",   900],
                ["MC",   1100],
                ["MCM",  1900],
                ["MM",   2000],
        ];
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