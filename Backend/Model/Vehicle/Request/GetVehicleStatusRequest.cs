using System.ComponentModel.DataAnnotations;

namespace Model.Vehicle.Request;

public sealed record GetVehicleStatusRequest
{
    [Required]
    public VehicleModelId VehicleId { get; set; }
}