namespace Fahrzeugverwaltung.Extensions;

public static class DateOnlyExtensions
{
    public static string ToIso8601(this DateOnly dateOnly)
    {
        return dateOnly.ToString("O");
    }
}