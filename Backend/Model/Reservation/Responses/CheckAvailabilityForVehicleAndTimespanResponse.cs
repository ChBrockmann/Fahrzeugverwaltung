using System.ComponentModel.DataAnnotations;

namespace Model.Reservation.Responses;

public record CheckAvailabilityForVehicleAndTimespanResponse
{
    [Required]
    public bool IsAvailable { get; set; }

    public List<ReservationModelDto>? BlockingReservations { get; set; } = new();
    
    public List<string> Errors { get; set; } = new();
}