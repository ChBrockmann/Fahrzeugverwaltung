namespace ValidateRouteMileage.Model.Route;

public record RouteDistanceResult
{
    public int TotalDistanceInMeter { get; set; }

    public string OriginAdress { get; set; } = string.Empty;

    public string DestinationAdress { get; set; } = string.Empty;
}