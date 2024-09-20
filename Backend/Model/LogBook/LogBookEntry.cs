using Model.Reservation;
using Model.User;
using Model.Vehicle;
using StronglyTypedIds;

namespace Model.LogBook;

[StronglyTypedId(Template.Guid)]
public partial struct LogBookEntryId { }

public sealed record LogBookEntry : IDatabaseId<LogBookEntryId>
{
    public LogBookEntryId Id { get; set; } = LogBookEntryId.Empty;

    public int CurrentNumber { get; set; }

    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public UserModel CreatedBy { get; set; } = new();
    
    public int? EndMileageInKm { get; set; }

    public ReservationModel? AssociatedReservation { get; set; }
    public VehicleModel AssociatedVehicle { get; set; } = new();
}