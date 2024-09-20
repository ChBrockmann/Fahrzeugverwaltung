using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Model.Reservation;

namespace Model.LogBook.Requests;

public class CreateLogBookRequest
{
    public IFormFile ImageFile { get; set; } = null!;

    [Required]
    public ReservationId ReservationId { get; set; } = ReservationId.Empty;

    public int? EndMileageInKm { get; set; }
    public string? Description { get; set; } = string.Empty;
}