using DataAccess.BaseService;
using Model.ReservationStatus;

namespace DataAccess.ReservationStatusService;

public class ReservationStatusService : BaseService<ReservationStatusModel, ReservationStatusId>, IReservationStatusService
{
    public ReservationStatusService(DatabaseContext database) : base(database) { }
}