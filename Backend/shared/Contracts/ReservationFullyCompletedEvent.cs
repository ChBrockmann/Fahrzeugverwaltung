namespace Contracts;

public record ReservationFullyCompletedEvent
{
    public int TotalMileageForRoute { get; set; }
    public DateOnly StartDateInclusive { get; set; }
    public DateOnly EndDateInclusive { get; set; }

    public string OriginAdress { get; set; } = string.Empty;
    public string DestinationAdress { get; set; } = string.Empty;
}