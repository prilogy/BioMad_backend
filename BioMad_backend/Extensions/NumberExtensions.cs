namespace BioMad_backend.Extensions
{
    public static class NumberExtensions
    {
        public static bool IsBetween(this double num, double a, double b)
            => num >= a && num < b || num <= a && num > b;
    }
}