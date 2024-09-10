using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace App;

public record RomanNumber(int Value)
{
    private readonly int _value = Value;
    public int Value { get => _value; init => _value = value; }

    public static RomanNumber Parse(string input)
    {
        int value = 0;
        int prevDigit = 0;
        int pos = input.Length;
        List<string> errors = [];
        foreach (char c in input.Reverse())
        {
            pos -= 1;
            int digit;
            try
            {
                digit = DigitValue(c.ToString());
            }
            catch
            {
                errors.Add($"Invalid character '{c}' at index {pos}");
                continue;
            }
            if (digit != 0 && (prevDigit / digit > 10 || ShouldBeReplacedByRightDigit(digit, prevDigit) || ShouldBeReplacedByNextDigit(prevDigit, digit)))
            {
                errors.Add($"Invalid order '{c}' before {input[pos + 1]} at index {pos} in \"{input}\"");
            }

            value += digit >= prevDigit ? digit : -digit;
            prevDigit = digit;
        }

        if (errors.Count != 0)
        {
            throw new FormatException(string.Join("; ", errors));
        }
        return new(value);
    }

    public static int DigitValue(string digit) => digit switch
    {
        "N" => 0,
        "I" => 1,
        "V" => 5,
        "X" => 10,
        "L" => 50,
        "C" => 100,
        "D" => 500,
        "M" => 1000,
        _ => throw new ArgumentException($"{nameof(DigitValue)}: {nameof(RomanNumber)} has invalid value of {digit}", nameof(digit))
    };

    public RomanNumber Plus(RomanNumber other)
    {
        return this with { Value = Value + other.Value };
    }

    public string Plus(string other)
    {
        RomanNumber parsed = Parse(other);
        RomanNumber result = this with { Value = Value + parsed.Value };
        return result.ToString();
    }

    public override string ToString()
    {
        if (_value == 0) return "N";
        Dictionary<int, string> parts = new()
        {
            { 1000, "M" },
            { 900, "CM" },
            { 500, "D" },
            { 400, "CD" },
            { 100, "C" },
            { 90, "XC" },
            { 50, "L" },
            { 40, "XL" },
            { 10, "X" },
            { 9, "IX" },
            { 5, "V" },
            { 4, "IV" },
            { 1, "I" },
        };

        int tempValue = _value;
        StringBuilder sb = new StringBuilder();
        while (tempValue > 0)
        {
            foreach (var pair in parts)
            {
                if(tempValue >= pair.Key)
                {
                    tempValue -= pair.Key;
                    sb.Append(pair.Value);
                    break;
                }
            }
        }
        return sb.ToString();
    }

    private static bool ShouldBeReplacedByRightDigit(int leftDigitValue, int rightDigitValue)
    {
        return rightDigitValue - leftDigitValue == leftDigitValue;
    }

    private static bool ShouldBeReplacedByNextDigit(int firstValue, int secondValue)
    {
        return firstValue == secondValue && firstValue * 2 == GetNextDigitValue(firstValue);
    }

    private static int GetNextDigitValue(int value)
    {
        int[] values = [1, 5, 10, 50, 100, 500, 1000];
        if (value == values[^1]) return value;

        for (int i = 0; i < values.Length; i++)
        {
            if (value == values[i])
            {
                return values[i + 1]; 
            }
        }

        return values[0];
    }
}
