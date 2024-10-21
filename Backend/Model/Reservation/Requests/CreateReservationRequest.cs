using System.ComponentModel.DataAnnotations;
using Model.Vehicle;

namespace Model.Reservation.Requests;

public record CreateReservationRequest
{
    [Required]
    public DateOnly StartDateInclusive { get; set; }

    [Required]
    public DateOnly EndDateInclusive { get; set; }

    [Required]
    public string Reason { get; set; } = string.Empty;

    [Required]
    public VehicleModelId Vehicle { get; set; }

    [Required]
    public string DestinationAdress { get; set; } = string.Empty;

    [Required]
    public string OriginAdress { get; set; } = string.Empty;
}