using Microsoft.AspNetCore.Identity;
using Model.Reservation;
using Model.ReservationStatus;

namespace Model.User;


public class UserModel : IdentityUser<Guid>
{
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;

    public string Organization { get; set; } = string.Empty;


    public List<ReservationModel> ReservationsMadeByUser { get; set; } = new();
    public List<ReservationStatusModel> ReservationStatusChanges { get; set; } = new();
}