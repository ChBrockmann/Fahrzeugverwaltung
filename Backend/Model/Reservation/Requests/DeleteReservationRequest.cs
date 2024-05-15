using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Requests;

public record DeleteReservationRequest
{
    [Required]
    public ReservationId ReservationId { get; set; }
}