using System.ComponentModel.DataAnnotations;
using Model.ReservationStatus;
using Model.User;
using Model.Vehicle;

namespace Model.Reservation;

public sealed record ReservationModelDto
{
    public ReservationId Id { get; set; } = ReservationId.Empty;
    
    public DateOnly StartDateInclusive { get; set; } = DateOnly.MinValue;
    public DateOnly EndDateInclusive { get; set; } = DateOnly.MinValue;


    public DateTime ReservationCreated => ReservationStatus.MinBy(x => x.StatusChanged)?.StatusChanged ?? DateTime.MinValue;
    public UserDto ReservationMadeByUser => ReservationStatus.MinBy(x => x.StatusChanged)?.StatusChangedByUser ?? new();
    
    
    public VehicleModelDto VehicleReserved { get; set; } = new();
    
    public List<ReservationStatusModelDto> ReservationStatus { get; set; } = new();
    
}