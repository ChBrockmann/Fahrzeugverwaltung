using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Reservation.Responses;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class CheckAvailabilityForVehicleAndTimespanEndpoint : Endpoint<CheckAvailabilityForVehicleAndTimespanRequest, CheckAvailabilityForVehicleAndTimespanResponse>
{
    private readonly IReservationService _reservationService;
    private readonly IMapper _mapper;
    
    public CheckAvailabilityForVehicleAndTimespanEndpoint(IReservationService reservationService, IMapper mapper)
    {
        _reservationService = reservationService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("reservation/checkAvailability/{RequestedVehicleId}");
    }

    public override async Task HandleAsync(CheckAvailabilityForVehicleAndTimespanRequest req, CancellationToken ct)
    {
        IEnumerable<ReservationModel>? blockingReservations = await _reservationService.GetReservationsInTimespan(req.StartDateInclusive, req.EndDateInclusive, req.RequestedVehicleId);

        if (blockingReservations is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var mapped = _mapper.Map<List<ReservationModelDto>>(blockingReservations.ToList());
        
        CheckAvailabilityForVehicleAndTimespanResponse response = new()
        {
            BlockingReservations = mapped,
            Availability = mapped.Any()  ? Availability.NotAvailable : Availability.Available,
        };

        await SendOkAsync(response, ct);
    }
}