using Model.ReservationStatus;
using Model.User;
using Model.Vehicle;
using StronglyTypedIds;

namespace Model.Reservation;

[StronglyTypedId(Template.Guid)]
public partial struct ReservationId { }

public sealed record ReservationModel : IDatabaseId<ReservationId>
{
    public string Reason { get; set; } = string.Empty;
    public DateOnly StartDateInclusive { get; set; } = DateOnly.MinValue;
    public DateOnly EndDateInclusive { get; set; } = DateOnly.MinValue;
    public DateTime ReservationCreated { get; set; } = DateTime.Now;

    public VehicleModel VehicleReserved { get; set; } = new() {Id = VehicleModelId.Empty};

    public UserModel ReservationMadeByUser { get; set; } = new();
    public List<ReservationStatusModel> ReservationStatusChanges { get; set; } = new();
    public ReservationId Id { get; set; } = ReservationId.Empty;
}