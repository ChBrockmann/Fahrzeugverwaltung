using DataAccess.BaseService;
using Model.ReservationStatus;

namespace DataAccess.ReservationStatusService;

public interface IReservationStatusService : IBaseService<ReservationStatusModel, ReservationStatusId> { }