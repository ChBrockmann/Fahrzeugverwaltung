namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;

public sealed record Configuration
{
    public RabbitMqConfiguration RabbitMq { get; set; } = new();
}