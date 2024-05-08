using Model.Reservation;

namespace Model.Vehicle.Response;

public enum VehicleStatus
{
    Available,
    Reserved
}

public sealed record GetVehicleStatusResponse
{
    public VehicleModelDto Vehicle { get; set; } = new();

    public List<ReservationModelDto> UpcomingReservations { get; set; } = new();
    public ReservationModelDto? CurrentReservation { get; set; } = new();

    public VehicleStatus VehicleStatus => CurrentReservation is null ? VehicleStatus.Available : VehicleStatus.Reserved;
}