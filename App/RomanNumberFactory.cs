namespace App;

public class RomanNumberFactory
{
    public static RomanNumber Parse(string input)
    {
        int value = 0;
        int rightDigit = 0;

        ValidateInput(input);

        foreach (char c in input.Reverse())
        {
            int digit = DigitValue(c.ToString());
            value += (digit >= rightDigit) ? digit : -digit;
            rightDigit = digit;
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

    private static void ValidateInput(string input)
    {
        ValidateSymbols(input);
        ValidatePairs(input);
        ValidateFormat(input);
        ValidateSubs(input);
    }

    private static void ValidateSubs(string input)
    {
        HashSet<char> subs = [];
        for (int i = 0; i < input.Length - 1; ++i)
        {
            char c = input[i];
            if (DigitValue(c.ToString()) < DigitValue(input[i + 1].ToString()))
            {
                if (subs.Contains(c))
                {
                    throw new FormatException();
                }
                subs.Add(c);
            }
        }
    }

    private static void ValidateFormat(string input)
    {
        int maxDigit = 0;
        bool wasLess = false;
        bool wasMax = false;
        int pos = input.Length;
        foreach (char c in input.Reverse())
        {
            pos--;
            int digit = DigitValue(c.ToString());
            if (digit < maxDigit)
            {
                if (wasLess || wasMax)
                {
                    throw new FormatException($"Invalid sequence: more than 1 less digit before '{input[pos + 1]}'");
                }

                wasLess = true;
            }
            else if (digit == maxDigit)
            {
                wasMax = true;
                wasLess = false;
            }
            else
            {
                maxDigit = digit;
                wasLess = false;
                wasMax = false;
            }
        }
    }

    private static void ValidatePairs(string input)
    {
        for (int i = 0; i < input.Length - 1; ++i)
        {
            int rightDigit = DigitValue(input[i + 1].ToString());
            int leftDigit = DigitValue(input[i].ToString());
            if (leftDigit != 0 &&
                leftDigit < rightDigit &&
                (rightDigit / leftDigit > 10 ||
                    (leftDigit == 5 || leftDigit == 50 || leftDigit == 500)
                ))
            {
                throw new FormatException(
                    $"Invalid order '{input[i]}' before '{input[i + 1]}' at index {i} in \"{input}\"");
            }
        }
    }

    private static void ValidateSymbols(string input)
    {
        int pos = 0;
        foreach (char c in input)
        {
            try
            {
                DigitValue(c.ToString());
            }
            catch
            {
                throw new FormatException($"Invalid character '{c}' at index {pos}");
            }
            pos += 1;
        }
    }
}
