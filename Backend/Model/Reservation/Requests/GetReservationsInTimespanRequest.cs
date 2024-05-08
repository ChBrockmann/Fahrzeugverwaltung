using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Requests;

public sealed record GetReservationsInTimespanRequest
{
    [Required]
    public DateOnly StartDateInclusive { get; set; }
    
    [Required]
    public DateOnly EndDateInclusive { get; set; }
}