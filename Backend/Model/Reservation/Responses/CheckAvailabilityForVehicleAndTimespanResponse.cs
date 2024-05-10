using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Responses;


public enum Availability
{
    Available,
    NotAvailable
}


public record CheckAvailabilityForVehicleAndTimespanResponse
{
    [Required]
    public Availability Availability { get; set; }
    
    public List<ReservationModelDto>? BlockingReservations { get; set; } = new();
}