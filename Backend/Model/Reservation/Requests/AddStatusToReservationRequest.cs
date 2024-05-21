using System.ComponentModel.DataAnnotations;
using Model.ReservationStatus;

namespace Model.Reservation.Requests;

public record AddStatusToReservationRequest
{
    [Required]
    public ReservationId ReservationId { get; set; }

    [Required]
    public ReservationStatusEnum Status { get; set; }

    [MaxLength(512)]
    public string? Reason { get; set; }
}