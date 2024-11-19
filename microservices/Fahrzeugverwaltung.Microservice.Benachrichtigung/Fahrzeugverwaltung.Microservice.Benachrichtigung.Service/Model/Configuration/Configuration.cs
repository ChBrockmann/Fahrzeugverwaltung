namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model.Configuration;

public sealed record Configuration
{
    public RabbitMqConfiguration RabbitMq { get; set; } = new();
    public KeycloakConfiguration Keycloak { get; set; } = new();
}