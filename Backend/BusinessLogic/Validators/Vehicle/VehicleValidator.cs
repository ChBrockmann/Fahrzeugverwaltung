using DataAccess.VehicleService;
using Model.Vehicle;

namespace BusinessLogic.Validators.Vehicle;

public class VehicleValidator
{
    private readonly IVehicleService _vehicleService;
    
    public VehicleValidator(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }
    
    public async Task<bool> CheckIfVehicleExists(VehicleModelId vehicleId, CancellationToken ct)
    {
        return await _vehicleService.Exists(vehicleId);
    }
}