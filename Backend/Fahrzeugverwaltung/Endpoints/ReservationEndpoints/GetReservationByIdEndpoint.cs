using System.Security.Claims;
using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Reservation.Responses;

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
            CanDelete = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value == result.ReservationMadeByUser.Id.ToString()
        };
        await SendOkAsync(response, ct);
    }
}