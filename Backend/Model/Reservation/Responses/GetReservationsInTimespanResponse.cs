using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Responses;

public record GetReservationsInTimespanResponse
{
    [Required]
    public List<ReservationModelDto> Reservations { get; set; } = new();
}