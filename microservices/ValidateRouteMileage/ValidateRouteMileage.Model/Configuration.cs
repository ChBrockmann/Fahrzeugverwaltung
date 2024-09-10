namespace ValidateRouteMileage.Model;

public record Configuration
{
    public RabbitMqConfiguration RabbitMq { get; set; } = new();
}