using FS.Keycloak.RestApiClient.Authentication.Client;

namespace Fahrzeugverwaltung.Keycloak;

public interface IKeycloakClientFactory
{
    public AuthenticationHttpClient CreateClient();
}