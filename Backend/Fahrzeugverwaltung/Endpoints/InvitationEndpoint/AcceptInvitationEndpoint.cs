using DataAccess.InvitationService;
using DataAccess.OrganizationService;
using DataAccess.UserService;
using Fahrzeugverwaltung.Keycloak;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.ClientFactory;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Options;
using Model.Invitation;
using Model.Invitation.Requests;
using Model.Roles;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class AcceptInvitationEndpoint : Endpoint<AcceptInvitationRequest, EmptyResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly ILogger _logger;
    private readonly IValidator<AcceptInvitationRequest> _validator;
    private readonly IUserService _userService;
    private readonly IOrganizationService _organizationService;
    private readonly IKeycloakClientFactory _keycloakClientFactory;
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;

    public AcceptInvitationEndpoint(ILogger logger, IInvitationService invitationService,
        IValidator<AcceptInvitationRequest> validator, IUserService userService, IOrganizationService organizationService,
        IKeycloakClientFactory keycloakClientFactory, IOptionsMonitor<Configuration> optionsMonitor)
    {
        _logger = logger;
        _invitationService = invitationService;
        _validator = validator;
        _userService = userService;
        _organizationService = organizationService;
        _keycloakClientFactory = keycloakClientFactory;
        _optionsMonitor = optionsMonitor;
    }

    public override void Configure()
    {
        Post("invitation/accept");
        AllowAnonymous();
        Throttle(100, new TimeSpan(1, 0, 0, 0).TotalSeconds);
    }

    public override async Task HandleAsync(AcceptInvitationRequest req, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid)
        {
            ValidationFailures.AddRange(validationResult.Errors);
            ThrowIfAnyErrors();
        }

        _logger.Information("Accepting invitation with token {Token}", req.Token);

        InvitationModel invitation = await _invitationService.GetByToken(req.Token) ?? throw new ArgumentNullException();
        _logger.Information("Token {Token} is valid. Request data: {0} {1} {2}", req.Token, req.Firstname, req.Lastname, req.Organization);

        UserModel result = await CreateUserInDatabase(req, invitation);

        _logger.Information("User created successfully in database");

        await CreateUserInKeycloak(result, invitation);

        await _invitationService.SetAcceptedByUser(invitation.Id, result);

        await SendOkAsync(ct);
    }

    private async Task CreateUserInKeycloak(UserModel databaseUser, InvitationModel invitation)
    {
        var client = _keycloakClientFactory.CreateClient();
        var usersApi = ApiClientFactory.Create<UsersApi>(client);
        var rolesApi = ApiClientFactory.Create<RoleMapperApi>(client);
        var config = _optionsMonitor.CurrentValue.Keycloak;

        string localDatabaseUserId = "LocalDatabaseUserId";

        await usersApi.PostUsersAsync(config.Realm, new UserRepresentation()
        {
            Email = databaseUser.Email,
            FirstName = databaseUser.Firstname,
            LastName = databaseUser.Lastname,
            Enabled = true,
            Attributes = new Dictionary<string, List<string>>()
            {
                {
                    localDatabaseUserId,
                    new List<string>()
                    {
                        databaseUser.Id.ToString()
                    }
                }
            }
        });

        var userFromKeycloak = (await usersApi.GetUsersAsync(config.Realm, q: $"{localDatabaseUserId}:{databaseUser.Id}")).SingleOrDefault();
        if (userFromKeycloak is null)
        {
            ThrowError("User not found in keycloak");
            return;
        }

        List<RoleRepresentation> roles = invitation.Roles.Select(role => GetOrCreateRoleRepresentation(role.Name).Result).ToList();
        
        await rolesApi.PostUsersRoleMappingsRealmByUserIdAsync(config.Realm, userFromKeycloak.Id, roles);

        await _userService.SetAuthIdOfUser(databaseUser.Id, userFromKeycloak.Id);

        await usersApi.PutUsersExecuteActionsEmailByUserIdAsync(config.Realm, userFromKeycloak.Id, requestBody: new List<string> {"UPDATE_PASSWORD"});
    }

    private async Task<UserModel> CreateUserInDatabase(AcceptInvitationRequest req, InvitationModel invitation)
    {
        var result = await _userService.Create(new UserModel
        {
            Id = UserId.New(),
            Firstname = req.Firstname,
            Lastname = req.Lastname,
            PhoneNumber = req.PhoneNumber,
            Email = req.Email,
            Organization = await _organizationService.GetOrCreate(req.Organization)
        });
        
        await _userService.SetRolesOfUser(result.Id, invitation.Roles);

        return result;
    }

    private async Task<RoleRepresentation> GetOrCreateRoleRepresentation(string roleName)
    {
        var client = _keycloakClientFactory.CreateClient();
        var clientApi = ApiClientFactory.Create<RolesApi>(client);
        var config = _optionsMonitor.CurrentValue.Keycloak;

        RoleRepresentation? result;
        try
        {
            result = await clientApi.GetRolesByRoleNameAsync(config.Realm, roleName);
        }
        catch (Exception)
        {
            await clientApi.PostRolesAsync(config.Realm, new RoleRepresentation(name: roleName, clientRole: true));
            result = await clientApi.GetRolesByRoleNameAsync(config.Realm, roleName);
        }

        return result;
    }
}