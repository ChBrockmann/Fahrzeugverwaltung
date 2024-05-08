namespace Model.Vehicle.Response;

public sealed record GetAllVehicleResponse
{
    public List<VehicleModelDto> Vehicles { get; set; } = new();
}