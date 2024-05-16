using System.ComponentModel.DataAnnotations.Schema;
using Model.ReservationStatus;
using Model.User;
using Model.Vehicle;
using StronglyTypedIds;

namespace Model.Reservation;

[StronglyTypedId(Template.Guid)]
public partial struct ReservationId { }

public sealed record ReservationModel : IDatabaseId<ReservationId>
{
    public ReservationId Id { get; set; } = ReservationId.Empty;

    public DateOnly StartDateInclusive { get; set; } = DateOnly.MinValue;
    public DateOnly EndDateInclusive { get; set; } = DateOnly.MinValue;
    
    
    public VehicleModel VehicleReserved { get; set; } = new() {Id = VehicleModelId.Empty};
    
    
    public List<ReservationStatusModel> ReservationStatus { get; set; } = new();
}