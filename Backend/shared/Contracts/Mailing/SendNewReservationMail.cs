using Model.Reservation;

namespace Contracts.Mailing;

public record SendNewReservationMail
{
    public ReservationId ReservationId { get; set; } = new();
}