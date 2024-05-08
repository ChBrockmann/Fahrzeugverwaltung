using System.ComponentModel.DataAnnotations;
using Model.User;
using Model.Vehicle;

namespace Model.Reservation;

public sealed record ReservationModelDto
{
    public ReservationId Id { get; set; } = ReservationId.Empty;
    
    public DateOnly StartDateInclusive { get; set; } = DateOnly.MinValue;
    public DateOnly EndDateInclusive { get; set; } = DateOnly.MinValue;
    public DateTime ReservationCreated { get; set; } = DateTime.Now;
    
    
    public UserDto ReservationMadeByUser { get; set; } = new();
    public VehicleModelDto VehicleReserved { get; set; } = new();
    
}