namespace Model.Configuration;

public record ReservationRestrictions
{
    public ReservationRestrictions() : this(0, 0, 0, 0) { }

    public ReservationRestrictions(int minReservationDays = 0, int maxReservationDays = 0, int minReservationTimeInAdvanceInDays = 0, int maxReservationTimeInAdvanceInDays = 0)
    {
        MinReservationDays = minReservationDays;
        MaxReservationDays = maxReservationDays;
        MinReservationTimeInAdvanceInDays = minReservationTimeInAdvanceInDays;
        MaxReservationTimeInAdvanceInDays = maxReservationTimeInAdvanceInDays;
    }

    public int MinReservationDays { get; set; }
    public int MaxReservationDays { get; set; }

    public int MinReservationTimeInAdvanceInDays { get; set; }
    public int MaxReservationTimeInAdvanceInDays { get; set; }
}