using System.Security.Claims;
using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Reservation.Responses;
using Model.ReservationStatus;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class GetReservationByIdEndpoint : Endpoint<GetReservationByIdRequest, GetReservationByIdResponse>
{
    private readonly IMapper _mapper;
    private readonly IReservationService _reservationService;

    public GetReservationByIdEndpoint(IMapper mapper, IReservationService reservationService)
    {
        _mapper = mapper;
        _reservationService = reservationService;
    }

    public override void Configure()
    {
        Get("reservation/{ReservationId}");
    }

    public override async Task HandleAsync(GetReservationByIdRequest req, CancellationToken ct)
    {
        ReservationModel? result = await _reservationService.Get(req.ReservationId);

        if (result is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        GetReservationByIdResponse response = new()
        {
            Reservation = _mapper.Map<ReservationModelDto>(result),
            CanDelete = CanDelete(result),
            CanChangeStatus = CanChangeStatus(result)
        };
        await SendOkAsync(response, ct);
    }

    private bool CanDelete(ReservationModel reservationModel)
    {
        return User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value == reservationModel.ReservationMadeByUser.Id.ToString();
    }

    private bool CanChangeStatus(ReservationModel reservation)
    {
        return User.IsInRole(Model.Security.AdminRoleName) &&
               reservation.ReservationStatusChanges.MaxBy(x => x.StatusChanged)?.Status != ReservationStatusEnum.Confirmed;
    }
}