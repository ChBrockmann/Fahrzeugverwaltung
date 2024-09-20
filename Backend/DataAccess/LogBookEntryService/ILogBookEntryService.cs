using DataAccess.BaseService;
using Model.LogBook;
using Model.Vehicle;

namespace DataAccess.LogBookEntryService;

public interface ILogBookEntryService : IBaseService<LogBookEntry, LogBookEntryId>
{
    public Task<LogBookEntry?> SetEndMileage(LogBookEntryId id, int mileageInKm);

    public Task<IEnumerable<LogBookEntry>> GetAllForVehicle(VehicleModelId vehicleModelId);
}