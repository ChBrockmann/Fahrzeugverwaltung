using DataAccess.BaseService;
using Microsoft.EntityFrameworkCore;
using Model.LogBook;
using Model.Vehicle;

namespace DataAccess.LogBookEntryService;

public class LogBookEntryService : BaseService<LogBookEntry, LogBookEntryId>, ILogBookEntryService
{
    public LogBookEntryService(DatabaseContext database) : base(database) { }

    public override async Task<IEnumerable<LogBookEntry>> Get()
    {
        return await Database.LogBookEntries
            .Include(x => x.AssociatedVehicle)
            .Include(x => x.AssociatedReservation)
            .ToListAsync();
    }

    public override async Task<LogBookEntry?> Get(LogBookEntryId id)
    {
        return await Database.LogBookEntries
            .Include(x => x.AssociatedVehicle)
            .Include(x => x.AssociatedReservation)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public override async Task<LogBookEntry> Create(LogBookEntry objectToCreate)
    {
        if (objectToCreate.Id == LogBookEntryId.Empty) objectToCreate.Id = LogBookEntryId.New();

        objectToCreate.CurrentNumber = (await GetCurrentNumber(objectToCreate.AssociatedVehicle.Id)) + 1;

        Database.LogBookEntries.Add(objectToCreate);
        await Database.SaveChangesAsync();

        return (await Get(objectToCreate.Id))!;
    }

    private async Task<int> GetCurrentNumber(VehicleModelId vehicleId)
    {
        LogBookEntry? x = await Database.LogBookEntries
            .Where(x => x.AssociatedVehicle.Id == vehicleId)
            .OrderByDescending(x => x.CurrentNumber)
            .FirstOrDefaultAsync();
        return x?.CurrentNumber ?? 0;
    }

    public async Task<LogBookEntry?> SetEndMileage(LogBookEntryId id, int mileageInKm)
    {
        LogBookEntry? logbookEntry = await Database.LogBookEntries.FirstOrDefaultAsync(x => x.Id == id);

        if (logbookEntry is null) return null;

        logbookEntry.EndMileageInKm = mileageInKm;
        await Database.SaveChangesAsync();

        return logbookEntry;
    }

    public async Task<IEnumerable<LogBookEntry>> GetAllForVehicle(VehicleModelId vehicleModelId)
    {
        return await Database.LogBookEntries
            .Include(x => x.AssociatedReservation)
            .Include(x => x.AssociatedVehicle)
            .Where(x => x.AssociatedVehicle.Id == vehicleModelId)
            .ToListAsync();
    }
}