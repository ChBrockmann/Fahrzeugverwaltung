namespace Fahrzeugverwaltung.Provider.DateTimeProvider;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Today => DateTime.Today;

    public DateOnly DateToday => DateOnly.FromDateTime(DateTime.Today);
}