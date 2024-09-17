using System.Text;

namespace App;

public record RomanNumber(int Value)
{
    public RomanNumber(string input) : this(RomanNumberFactory.Parse(input)) {}
    
public string Plus(string other)
    {
        RomanNumber parsed = RomanNumberFactory.Parse(other);
        RomanNumber result = this with { Value = Value + parsed.Value };
        return result.ToString();
    }

    public override string ToString()
    {
        if (Value == 0) return "N";
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

        int tempValue = Value;
        StringBuilder sb = new();
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
}
