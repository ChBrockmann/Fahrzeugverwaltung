using DataAccess.BaseService;
using Model.ReservationStatus;

namespace DataAccess.ReservationStatusService;

public class ReservationStatusService : BaseService<ReservationStatusModel, ReservationStatusId>, IReservationStatusService
{
    public ReservationStatusService(DatabaseContext database) : base(database) { }

    public async Task AddStatusToReservationAsync(ReservationStatusModel model, CancellationToken ct)
    {
        model.Id = ReservationStatusId.New();
        Database.ReservationStatusModels.Add(model);
        await Database.SaveChangesAsync(ct);
    }
}