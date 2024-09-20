using System.ComponentModel.DataAnnotations;
using Model.Vehicle;

namespace Model.LogBook.Requests;

public record CreateManualLogBookEntryRequest
{
    [Required]
    public VehicleModelId VehicleModelId { get; set; }

    [Required]
    public int TotalMileageInKm { get; set; }

    public string? Description { get; set; } = string.Empty;
}