using Model.Reservation;

namespace Contracts.Notification;

public sealed record NewReservationCreatedEvent
{
    public ReservationId ReservationId { get; set; } = new();
}