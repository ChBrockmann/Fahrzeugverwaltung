namespace Model.Vehicle;

public sealed record VehicleModelDto
{
    public VehicleModelId Id { get; set; }

    public string Name { get; set; } = string.Empty;
}