using DataAccess.VehicleService;
using Model.Vehicle;
using Model.Vehicle.Response;

namespace Fahrzeugverwaltung.Endpoints.VehicleEndpoint;

public class GetAllVehicleEndpoint : Endpoint<EmptyRequest, GetAllVehicleResponse>
{
    private readonly IVehicleService _vehicleService;
    private readonly IMapper _mapper;
    
    public GetAllVehicleEndpoint(IVehicleService vehicleService, IMapper mapper)
    {
        _vehicleService = vehicleService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("vehicle");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var allVehicles = await _vehicleService.Get();
        GetAllVehicleResponse response = new()
        {
            Vehicles = _mapper.Map<List<VehicleModelDto>>(allVehicles)
        };

        await SendOkAsync(response);
    }
}