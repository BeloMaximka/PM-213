using App;
using FluentAssertions;
using System.Reflection;

namespace Test;

[TestClass]
public class RomanNumberFactoryTests
{
    [TestMethod]
    [DataRow("N", 0)]
    [DataRow("I", 1)]
    [DataRow("II", 2)]
    [DataRow("III", 3)]
    [DataRow("IIII", 4)]
    [DataRow("IV", 4)]
    [DataRow("VI", 6)]
    [DataRow("VII", 7)]
    [DataRow("VIII", 8)]
    [DataRow("IX", 9)]
    [DataRow("D", 500)]
    [DataRow("M", 1000)]
    [DataRow("CM", 900)]
    [DataRow("MC", 1100)]
    [DataRow("MCM", 1900)]
    [DataRow("MM", 2000)]
    public void GivenCorrectRomanNumbers_ShouldParseWithCorrectValue(string romanNumber, int expected)
    {
        RomanNumber rn = RomanNumberFactory.Parse(romanNumber);
        rn.Value.Should().Be(expected);
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
    public void GivenIncorrectDigitSequence_WhenParsing_ShouldThrowFormatException(string input, char characterAfterIncorrect)
    {
        var act = () => RomanNumberFactory.Parse(input);
        act.Should().Throw<FormatException>().WithMessage($"Invalid sequence: more than 1 less digit before '{characterAfterIncorrect}'");
    }

    [TestMethod]
    [DataRow("IXIX")]
    [DataRow("IXX")]
    [DataRow("IXIV")]
    [DataRow("XCXC")]
    [DataRow("CMM")]
    [DataRow("CMCD")]
    [DataRow("XCXL")]
    [DataRow("XCC")]
    [DataRow("XCCI")]
    public void GivenIncorrectNumberFormat_WhenParsing_ShouldThrowFormatException(string input)
    {
        var act = () => RomanNumberFactory.Parse(input);
        act.Should().Throw<FormatException>();
    }

    [TestMethod]
    [DataRow("N", 0)]
    [DataRow("I", 1)]
    [DataRow("V", 5)]
    [DataRow("X", 10)]
    [DataRow("L", 50)]
    [DataRow("C", 100)]
    [DataRow("D", 500)]
    [DataRow("M", 1000)]
    public void GivenRomanDigit_ShouldReturnCorrectInteger(string romanDigit, int expectedNumber)
    {
        RomanNumberFactory.DigitValue(romanDigit).Should().Be(expectedNumber);
    }

    [TestMethod]
    public void GivenIncorrectDigit_ShouldThrowException()
    {
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
}
