using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Reservation.Responses;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class GetReservationsInTimespanEndpoint : Endpoint<GetReservationsInTimespanRequest, GetReservationsInTimespanResponse>
{
    private readonly ILogger _logger;
    private readonly IMapper _mapper;
    private readonly IReservationService _reservationService;

    public GetReservationsInTimespanEndpoint(IMapper mapper, IReservationService reservationService, ILogger logger)
    {
        _mapper = mapper;
        _reservationService = reservationService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("reservation/all");
    }

    public override async Task HandleAsync(GetReservationsInTimespanRequest req, CancellationToken ct)
    {
        _logger.Information("GetAllReservationsInTimespan Startdate: {StartDate} Enddate: {EndDate}",
            req.StartDateInclusive.ToString("O"), req.EndDateInclusive.ToString("O"));
        IEnumerable<ReservationModel> reservations = await _reservationService.GetReservationsInTimespan(req.StartDateInclusive, req.EndDateInclusive);

        GetReservationsInTimespanResponse response = new GetReservationsInTimespanResponse
        {
            Reservations = _mapper.Map<List<ReservationModelDto>>(reservations)
        };
        await SendOkAsync(response, ct);
    }
}