using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Reservation.Responses;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class GetReservationsInMonthYearEndpoint : Endpoint<GetReservationsInMonthYearRequest, GetReservationsInMonthYearResponse>
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;
    
    public GetReservationsInMonthYearEndpoint(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("reservation/all/{Year}/{Month}");
    }

    public override async Task HandleAsync(GetReservationsInMonthYearRequest req, CancellationToken ct)
    {
        IEnumerable<ReservationModel> allReservations = await _reservationService.GetReservationsInMonthYear(req.Year, req.Month);

        GetReservationsInMonthYearResponse response = new();
        response.Reservations = _mapper.Map<List<ReservationModelDto>>(allReservations);

        await SendOkAsync(response, ct);
    }
}