namespace App;

public static class RomanNumberMath
{
    public static RomanNumber Sum(params RomanNumber[] arguments)
    {
        return new RomanNumber(arguments.Sum(rn => rn.Value));
    }
}
