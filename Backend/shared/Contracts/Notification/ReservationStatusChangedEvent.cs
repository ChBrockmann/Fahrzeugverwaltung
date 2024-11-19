using Model.Reservation;

namespace Contracts.Notification;

public sealed record ReservationStatusChangedEvent
{
    public ReservationId ReservationId { get; set; }
}