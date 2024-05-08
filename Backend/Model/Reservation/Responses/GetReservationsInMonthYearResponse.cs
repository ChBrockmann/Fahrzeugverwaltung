using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Responses;

public record GetReservationsInMonthYearResponse
{
    [Required]
    public List<ReservationModelDto> Reservations { get; set; } = new();
}