using App;
using FluentAssertions;
using System.Reflection;

namespace Test;

[TestClass]
public class RomanNumberFactoryTests
{
    [TestMethod]
    [DynamicData(nameof(ValidNumberPairTestCases))]
    public void ParseTest(string romanNumber, int number)
    {
        RomanNumber rn = RomanNumberFactory.Parse(romanNumber);
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
        var act = () => RomanNumberFactory.Parse(input);
        act.Should().Throw<FormatException>().WithMessage($"Invalid character '{incorrectCharacter}' at index {position}");
    }

    [TestMethod]
    [DataRow("IM", 'I', 'M', 0)]
    [DataRow("IMX", 'I', 'M', 0)]
    [DataRow("XMD", 'X', 'M', 0)]
    [DataRow("LC", 'L', 'C', 0)]
    public void GivenIncorrectNumberFormat_WhenParsing_ShouldThrowFormatExceptionWithMessage(string input, char incorrectCharacter, char correctCharacter, int position)
    {
        var act = () => RomanNumberFactory.Parse(input);
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
        var act = () => RomanNumberFactory.Parse(input);
        act.Should().Throw<FormatException>().WithMessage($"Invalid sequence: more than 1 less digit before '{characterAfterIncorrect}'");
    }

    [TestMethod]
    [DynamicData(nameof(ValidDigitPairTestCases))]
    public void DigitValueTest(string romanDigit, int expectedNumber)
    {

        RomanNumberFactory.DigitValue(romanDigit).Should().Be(expectedNumber);
        var act = () => RomanNumberFactory.DigitValue("IncorrectTestValue");
        var ex = act.Should().Throw<ArgumentException>().WithParameterName("digit").And;
        ex.Message.Should().NotBeNullOrEmpty();
        ex.Message.Should().Contain("'digit'");
        ex.Message.Should().Contain(nameof(RomanNumber));
        ex.Message.Should().Contain(nameof(RomanNumberFactory.DigitValue));
        ex.Message.Should().Contain("IncorrectTestValue");
    }

    [TestMethod]
    [DataRow("IXIX")]
    [DataRow("IXX")]
    [DataRow("IVIV")]
    [DataRow("XCC")]
    public void ValidateInputTest(string input)
    {
        Type rnType = typeof(RomanNumberFactory);
        MethodInfo? methodInfo = rnType.GetMethod("ValidateInput", BindingFlags.NonPublic | BindingFlags.Static);

        var act = () => methodInfo?.Invoke(null, [input]);

        var exception = act.Should().Throw<TargetInvocationException>().And;
        exception.InnerException.Should().BeOfType<FormatException>();
    }

    [TestMethod]
    public void ValidatePairsTest()
    {
        Type rnType = typeof(RomanNumberFactory);
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
        Type rnType = typeof(RomanNumberFactory);
        MethodInfo? methodInfo = rnType.GetMethod("ValidateSubs", BindingFlags.NonPublic | BindingFlags.Static);

        var act = () => methodInfo?.Invoke(null, [input]);
        var exception = act.Should().Throw<TargetInvocationException>().And;
        exception.InnerException.Should().BeOfType<FormatException>();
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
