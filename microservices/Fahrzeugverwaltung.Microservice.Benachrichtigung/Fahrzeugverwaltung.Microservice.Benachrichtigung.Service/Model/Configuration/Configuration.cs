namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model.Configuration;

public sealed record Configuration
{
    public string DatabaseConnectionString { get; set; } = string.Empty;
    public RabbitMqConfiguration RabbitMq { get; set; } = new();
    public KeycloakConfiguration Keycloak { get; set; } = new();
}