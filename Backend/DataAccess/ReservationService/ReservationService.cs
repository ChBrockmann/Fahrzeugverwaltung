using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;
using Model.Reservation;
using Model.ReservationStatus;
using Model.Vehicle;

namespace DataAccess.ReservationService;

public class ReservationService : BaseService<ReservationModel, ReservationId>, IReservationService
{
    public ReservationService(DatabaseContext database) : base(database) { }

    public override async Task<ReservationModel> Create(ReservationModel reservation)
    {
        reservation.Id = ReservationId.New();

        Database.ReservationModels.Add(reservation);

        await Database.SaveChangesAsync();

        ReservationModel databaseObject = await Database.ReservationModels
            .Include(x => x.ReservationStatusChanges)
            .FirstAsync(x => x.Id == reservation.Id);
        databaseObject.ReservationStatusChanges = new List<ReservationStatusModel>
        {
            new()
            {
                Id = ReservationStatusId.New(),
                StatusChangedByUser = reservation.ReservationMadeByUser,
                Reservation = reservation,
                Status = ReservationStatusEnum.Pending,
                StatusChanged = reservation.ReservationCreated
            }
        };

        await Database.SaveChangesAsync();

        return reservation;
    }

    public async Task<IEnumerable<ReservationModel>> GetReservationsInMonthYear(int year, int month)
    {
        DateOnly start = new(year, month, 1);
        DateOnly end = start.AddMonths(1).AddDays(-1);
        return await GetReservationsInTimespan(start, end);
    }

    public override async Task<ReservationModel?> Get(ReservationId id)
    {
        return await Database.ReservationModels
            .Include(x => x.VehicleReserved)
            .Include(x => x.ReservationMadeByUser)
            .Include(x => x.ReservationStatusChanges)
            .ThenInclude(x => x.StatusChangedByUser)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<IEnumerable<ReservationModel>> Get()
    {
        return await Database.ReservationModels
            .Include(x => x.VehicleReserved)
            .Include(x => x.ReservationMadeByUser)
            .Include(x => x.ReservationStatusChanges)
            .ThenInclude(x => x.StatusChangedByUser)
            .ToListAsync();
    }

    public async Task<IEnumerable<ReservationModel>> GetReservationsInTimespan(DateOnly queryStartDateInclusive, DateOnly queryEndDateInclusive)
    {
        return await Database.ReservationModels
            .Include(x => x.VehicleReserved)
            .Include(x => x.ReservationMadeByUser)
            .Include(x => x.ReservationStatusChanges)
            .ThenInclude(x => x.StatusChangedByUser)
            .Where(x => (x.StartDateInclusive >= queryStartDateInclusive && x.StartDateInclusive <= queryEndDateInclusive) ||
                        (x.EndDateInclusive >= queryStartDateInclusive && x.EndDateInclusive <= queryEndDateInclusive) ||
                        (x.StartDateInclusive < queryStartDateInclusive && x.EndDateInclusive > queryEndDateInclusive))
            .ToListAsync();
    }

    public async Task<IEnumerable<ReservationModel>?> GetReservationsInTimespan(DateOnly queryStartDateInclusive, DateOnly queryEndDateInclusive, VehicleModelId vehicleId)
    {
        return await Database.ReservationModels
            .Include(x => x.VehicleReserved)
            .Include(x => x.ReservationMadeByUser)
            .Include(x => x.ReservationStatusChanges)
            .ThenInclude(x => x.StatusChangedByUser)
            .Where(x => x.VehicleReserved.Id == vehicleId)
            .Where(x => (x.StartDateInclusive >= queryStartDateInclusive && x.StartDateInclusive <= queryEndDateInclusive) ||
                        (x.EndDateInclusive >= queryStartDateInclusive && x.EndDateInclusive <= queryEndDateInclusive) ||
                        (x.StartDateInclusive < queryStartDateInclusive && x.EndDateInclusive > queryEndDateInclusive))
            .ToListAsync();
    }

    public async Task<IEnumerable<ReservationModel>> GetUpcomingReservationsForVehicle(VehicleModelId vehicleId, DateOnly date, int limit = 2)
    {
        return await Database.ReservationModels
            .Include(x => x.VehicleReserved)
            .Include(x => x.ReservationMadeByUser)
            .Include(x => x.ReservationStatusChanges)
            .ThenInclude(x => x.StatusChangedByUser)
            .Where(x => x.VehicleReserved.Id == vehicleId)
            .Where(x => x.StartDateInclusive > date)
            .Take(limit)
            .ToListAsync();
    }

    public async Task<ReservationModel?> GetCurrentReservationForVehicle(VehicleModelId vehicleId, DateOnly date)
    {
        return await Database.ReservationModels
            .Include(x => x.VehicleReserved)
            .Include(x => x.ReservationMadeByUser)
            .Include(x => x.ReservationStatusChanges)
            .ThenInclude(x => x.StatusChangedByUser)
            .Where(x => x.VehicleReserved.Id == vehicleId)
            .Where(x => x.EndDateInclusive >= date && x.StartDateInclusive <= date)
            .FirstOrDefaultAsync();
    }

    public async Task<ReservationModel?> UpdateReservation(ReservationModel reservation)
    {
        ReservationModel? existing = await Database.ReservationModels.FindAsync(reservation.Id);

        if (existing is null)
            return null;

        Database.Entry(existing).CurrentValues.SetValues(reservation);

        await Database.SaveChangesAsync();

        return await Database.ReservationModels.FindAsync(reservation.Id);
    }
}