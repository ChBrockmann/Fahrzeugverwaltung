using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Responses;

public record GetReservationByIdResponse
{
    [Required]
    public ReservationModelDto Reservation { get; set; } = new();

    [Required]
    public bool CanDelete { get; set; } = false;
}