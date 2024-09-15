using DataAccess;
using DataAccess.UserService;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;

namespace Fahrzeugverwaltung.Endpoints;

public class TestEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;
    private DatabaseContext _database;

    public TestEndpoint(ILogger logger, DatabaseContext database, IUserService userService)
    {
        _logger = logger;
        _database = database;
        _userService = userService;
    }

    public override void Configure()
    {
        Get("test");
        Roles("Fahrzeugwart", "Admin");
    }


    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        _logger.Information("Test Endpoint Called!");
        _logger.Information("User Object: {User}", User.Claims);
        _logger.Information("User: {User}", User.IsInRole("Admin"));
        _logger.Information("User: {User}", User.IsInRole("Fahrzeugwart"));
        _logger.Information("User: {User}", User.IsInRole("manage-account"));

        var credentials = new ClientCredentialsFlow()
        {
            Realm = "fahrzeugverwaltung",
            ClientId = "fahrzeugverwaltung-backend",
            ClientSecret = "MCA50iqn0MMdcXarJzh17WUJeuIOK91G",
            KeycloakUrl = "http://localhost:8080/"
        };
        using var httpClient = AuthenticationHttpClientFactory.Create(credentials);
        using var usersApi = FS.Keycloak.RestApiClient.ClientFactory.ApiClientFactory.Create<UsersApi>(httpClient);
        
        var users = await usersApi.GetUsersAsync("fahrzeugverwaltung", cancellationToken: ct);

        foreach (var user in users)
        {
            _logger.Information("User: {User} {UserId}", user.Username, user.Id);
        }
        
        
        
        await SendOkAsync(ct);
    }
}