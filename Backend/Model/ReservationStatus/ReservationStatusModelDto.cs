using Model.User;

namespace Model.ReservationStatus;

public record ReservationStatusModelDto
{
    public ReservationStatusId Id { get; set; }

    public ReservationStatusEnum Status { get; set; } = ReservationStatusEnum.Pending;

    public DateTime StatusChanged { get; set; } = DateTime.Now;
    public UserDto StatusChangedByUser { get; set; } = new();

    public string? StatusReason { get; set; } = string.Empty;
}