using Model.Organization;
using Model.Reservation;
using Model.ReservationStatus;
using Model.Roles;
using StronglyTypedIds;

namespace Model.User;

[StronglyTypedId(Template.Guid)]
public partial struct UserId { }

public class UserModel : IDatabaseId<UserId>
{
    public UserId Id { get; set; }
    public string AuthId { get; set; } = string.Empty;
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public OrganizationModel Organization { get; set; } = new();

    public List<Role> Roles { get; set; } = new();

    public List<ReservationModel> ReservationsMadeByUser { get; set; } = new();
    public List<ReservationStatusModel> ReservationStatusChanges { get; set; } = new();
}