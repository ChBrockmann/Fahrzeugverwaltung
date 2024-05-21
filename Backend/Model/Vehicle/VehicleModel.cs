using Model.Reservation;
using StronglyTypedIds;

namespace Model.Vehicle;

[StronglyTypedId(Template.Guid)]
public partial struct VehicleModelId { }

public sealed record VehicleModel
{
    public VehicleModelId Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public List<ReservationModel> Reservations { get; set; } = new();
}