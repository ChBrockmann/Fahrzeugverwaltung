namespace ValidateRouteMileage.Model.Configuration;

public sealed record GoogleMapsConfiguration
{
    public string ApiKey { get; set; } = string.Empty;
}