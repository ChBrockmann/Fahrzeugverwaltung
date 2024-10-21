using Model.Reservation;

namespace Contracts.Mailing;

public record SendReservationStatusChangedMail
{
    public ReservationId ReservationId { get; set; }
}