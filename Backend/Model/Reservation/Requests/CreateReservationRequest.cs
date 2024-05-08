using System.ComponentModel.DataAnnotations;
using Model.User;
using Model.Vehicle;

namespace Model.Reservation.Requests;

public record CreateReservationRequest
{
    [Required]
    public DateOnly StartDateInclusive { get; set; }
    
    [Required]
    public DateOnly EndDateInclusive { get; set; }
    
    [Required]
    public Guid ReservedBy { get; set; }
    
    [Required]
    public VehicleModelId Vehicle { get; set; }
}