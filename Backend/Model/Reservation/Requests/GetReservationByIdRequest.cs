using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Requests;

public record GetReservationByIdRequest
{
    [Required]
    public ReservationId ReservationId { get; set; }
}