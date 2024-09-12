namespace ValidateRouteMileage.Model.Configuration;

public sealed record Configuration
{
    public RabbitMqConfiguration RabbitMq { get; set; } = new();

    public GoogleMapsConfiguration GoogleMaps { get; set; } = new();
    
    public RouteValidationParametersConfiguration RouteValidationParameters { get; set; } = new();
}