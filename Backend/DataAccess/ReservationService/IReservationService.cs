using DataAccess.BaseService;
using Model.Reservation;
using Model.Vehicle;

namespace DataAccess.ReservationService;

public interface IReservationService : IBaseService<ReservationModel, ReservationId>
{
    public Task<IEnumerable<ReservationModel>> GetReservationsInMonthYear(int year, int month);
    public Task<IEnumerable<ReservationModel>> GetReservationsInTimespan(DateOnly queryStartDateInclusive, DateOnly queryEndDateInclusive);
    public Task<IEnumerable<ReservationModel>?> GetReservationsInTimespan(DateOnly queryStartDateInclusive, DateOnly queryEndDateInclusive, VehicleModelId vehicleId);
    public Task<IEnumerable<ReservationModel>?> GetReservationsInTimespanWithoutDenied(DateOnly queryStartDateInclusive, DateOnly queryEndDateInclusive, VehicleModelId vehicleId);
    public Task<IEnumerable<ReservationModel>> GetUpcomingReservationsForVehicle(VehicleModelId vehicleId, DateOnly date, int limit = 2);
    public Task<ReservationModel?> GetCurrentReservationForVehicle(VehicleModelId vehicleId, DateOnly date);

    public Task<ReservationModel?> UpdateReservation(ReservationModel reservation);
}