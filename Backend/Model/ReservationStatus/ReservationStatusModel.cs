using System.ComponentModel.DataAnnotations;
using Model.Reservation;
using Model.User;
using StronglyTypedIds;

namespace Model.ReservationStatus;

[StronglyTypedId(Template.Guid)]
public partial struct ReservationStatusId { }

public enum ReservationStatusEnum
{
    Pending = 1,
    Confirmed = 2,
    Denied = 3
}

public record ReservationStatusModel
{
    public ReservationStatusId Id { get; set; }

    public ReservationStatusEnum Status { get; set; } = ReservationStatusEnum.Pending;

    public DateTime StatusChanged { get; set; } = DateTime.Now;
    public UserModel StatusChangedByUser { get; set; } = new();

    [MaxLength(512)]
    public string? StatusReason { get; set; } = string.Empty;

    public ReservationModel Reservation { get; set; } = new();
}