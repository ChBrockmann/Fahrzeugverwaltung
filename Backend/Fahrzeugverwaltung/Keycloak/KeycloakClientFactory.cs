using FS.Keycloak.RestApiClient.Authentication.Client;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using Microsoft.Extensions.Options;
using Model.Configuration;

namespace Fahrzeugverwaltung.Keycloak;

public class KeycloakClientFactory : IKeycloakClientFactory
{
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;

    public KeycloakClientFactory(IOptionsMonitor<Configuration> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public AuthenticationHttpClient CreateClient()
    {
        KeycloakConfiguration configuration = _optionsMonitor.CurrentValue.Keycloak;

        ClientCredentialsFlow credentials = new ClientCredentialsFlow
        {
            Realm = configuration.Realm,
            ClientId = configuration.ClientId,
            ClientSecret = configuration.ClientSecret,
            KeycloakUrl = configuration.BaseAuthServerUrl
        };
        return AuthenticationHttpClientFactory.Create(credentials);
    }
}