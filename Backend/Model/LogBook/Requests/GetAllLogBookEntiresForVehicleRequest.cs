using System.ComponentModel.DataAnnotations;
using Model.Vehicle;

namespace Model.LogBook.Requests;

public record GetAllLogBookEntiresForVehicleRequest
{
    [Required]
    public VehicleModelId VehicleModelId { get; set; }
}