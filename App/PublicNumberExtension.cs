namespace App;

public static class PublicNumberExtension
{
    public static RomanNumber Sum(this RomanNumber first, params RomanNumber[] args)
    {
        return RomanNumberMath.Sum([first, ..args]);
    }
}
