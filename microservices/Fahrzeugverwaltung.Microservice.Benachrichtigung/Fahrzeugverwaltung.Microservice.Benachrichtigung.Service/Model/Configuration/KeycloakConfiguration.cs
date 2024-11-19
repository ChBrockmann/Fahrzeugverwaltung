namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model.Configuration;

public sealed record KeycloakConfiguration
{
    public const string SectionName = "Keycloak";

    public string Realm { get; set; } = string.Empty;

    public string BaseAuthServerUrl { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;
}