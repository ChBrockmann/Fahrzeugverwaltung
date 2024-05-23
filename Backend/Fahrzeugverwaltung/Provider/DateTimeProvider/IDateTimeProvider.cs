namespace Fahrzeugverwaltung.Provider.DateTimeProvider;

public interface IDateTimeProvider
{
    public DateTime Now { get; }
    public DateTime UtcNow { get; }
    public DateTime Today { get; }

    public DateOnly DateToday { get; }
}