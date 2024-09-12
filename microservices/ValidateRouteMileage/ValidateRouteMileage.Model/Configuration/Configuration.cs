namespace ValidateRouteMileage.Model.Configuration;

public record Configuration
{
    public RabbitMqConfiguration RabbitMq { get; set; } = new();

    public GoogleMapsConfiguration GoogleMaps { get; set; } = new();
}