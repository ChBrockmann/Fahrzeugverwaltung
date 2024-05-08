using DataAccess.ReservationService;
using DataAccess.VehicleService;
using Model.Reservation;
using Model.Vehicle;
using Model.Vehicle.Request;
using Model.Vehicle.Response;

namespace Fahrzeugverwaltung.Endpoints.VehicleEndpoint;

public class GetVehicleStatusEndpoint : Endpoint<GetVehicleStatusRequest, GetVehicleStatusResponse>
{
    private readonly IReservationService _reservationService;
    private readonly IVehicleService _vehicleService;
    private readonly IMapper _mapper;
    
    public GetVehicleStatusEndpoint(IReservationService reservationService, IMapper mapper, IVehicleService vehicleService)
    {
        _reservationService = reservationService;
        _mapper = mapper;
        _vehicleService = vehicleService;
    }

    public override void Configure()
    {
        Get("vehicle/status/{VehicleId}");
    }

    public override async Task HandleAsync(GetVehicleStatusRequest req, CancellationToken ct)
    {
        VehicleModel? vehicle = await _vehicleService.Get(req.VehicleId);
        if (vehicle is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        var currentReservation = await _reservationService.GetCurrentReservationForVehicle(req.VehicleId, today);
        var upcomingReservations = await _reservationService.GetUpcomingReservationsForVehicle(req.VehicleId, today);
        
        GetVehicleStatusResponse response = new()
        {
            Vehicle = _mapper.Map<VehicleModelDto>(vehicle),
            CurrentReservation = currentReservation is null ? null : _mapper.Map<ReservationModelDto>(currentReservation),
            UpcomingReservations = _mapper.Map<List<ReservationModelDto>>(upcomingReservations)
        };

        await SendOkAsync(response, ct);
    }
}