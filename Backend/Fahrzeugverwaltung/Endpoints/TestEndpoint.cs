using Contracts;
using DataAccess;
using DataAccess.UserService;
using Fahrzeugverwaltung.Keycloak;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.ClientFactory;
using FS.Keycloak.RestApiClient.Authentication.Flow;
using FS.Keycloak.RestApiClient.ClientFactory;
using MassTransit;

namespace Fahrzeugverwaltung.Endpoints;

public class TestEndpoint : BaseEndpoint<EmptyRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IUserService _userService;
    private DatabaseContext _database;
    private IKeycloakClientFactory _keycloakClientFactory;
    private readonly IPublishEndpoint _publishEndpoint;

    public TestEndpoint(ILogger logger, DatabaseContext database, IUserService userService, IKeycloakClientFactory keycloakClientFactory, IPublishEndpoint publishEndpoint)
    {
        _logger = logger;
        _database = database;
        _userService = userService;
        _keycloakClientFactory = keycloakClientFactory;
        _publishEndpoint = publishEndpoint;
    }

    public override void Configure()
    {
        Get("test");
        Roles("Fahrzeugwart", "Admin");
    }


    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var client = _keycloakClientFactory.CreateClient();
        var usersApi = ApiClientFactory.Create<UsersApi>(client);
        
        _logger.Information("User: {Firstname} {Lastname}", UserFromContext.Firstname, UserFromContext.Lastname);
        _logger.Information("Test Endpoint Called!");
        _logger.Information("User Object: {User}", User.Claims);
        _logger.Information("User: {User}", User.IsInRole("Admin"));
        _logger.Information("User: {User}", User.IsInRole("Fahrzeugwart"));
        _logger.Information("User: {User}", User.IsInRole("manage-account"));

        await _publishEndpoint.Publish(new NotifyUserEvent()
        {
            Message = "Test Message"
        }, ct);

        
        
        // var users = await usersApi.GetUsersAsync("fahrzeugverwaltung", cancellationToken: ct);
        //
        // foreach (var user in users)
        // {
        //     _logger.Information("User: {User} {UserId}", user.Username, user.Id);
        // }
        
        
        
        await SendOkAsync(ct);
    }
}