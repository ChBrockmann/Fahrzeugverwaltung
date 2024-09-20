using Model.ReservationStatus;
using Model.User;
using Model.Vehicle;

namespace Model.Reservation;

public sealed record ReservationModelDto
{
    public ReservationId Id { get; set; } = ReservationId.Empty;

    public string Reason { get; set; } = string.Empty;
    public DateOnly StartDateInclusive { get; set; } = DateOnly.MinValue;
    public DateOnly EndDateInclusive { get; set; } = DateOnly.MinValue;

    public string OriginAdress { get; set; } = string.Empty;
    public string DestinationAdress { get; set; } = string.Empty;

    public DateTime ReservationCreated { get; set; } = DateTime.Now;
    public UserDto ReservationMadeByUser { get; set; } = new();

    public ReservationStatusEnum CurrentStatus => ReservationStatusChanges.MaxBy(x => x.StatusChanged)?.Status ?? ReservationStatusEnum.Pending;

    public VehicleModelDto VehicleReserved { get; set; } = new();
    public List<ReservationStatusModelDto> ReservationStatusChanges { get; set; } = new();
}