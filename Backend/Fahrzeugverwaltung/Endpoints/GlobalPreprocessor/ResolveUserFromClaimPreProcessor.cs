using System.Security.Claims;
using DataAccess;
using Fahrzeugverwaltung.Keycloak;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.Client;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Organization;
using Model.User;
using ApiClientFactory = FS.Keycloak.RestApiClient.ClientFactory.ApiClientFactory;

namespace Fahrzeugverwaltung.Endpoints.GlobalPreprocessor;

public class ResolveUserFromClaimPreProcessor : IGlobalPreProcessor
{
    private readonly ILogger _logger;

    public ResolveUserFromClaimPreProcessor(ILogger logger)
    {
        _logger = logger;
    }

    public async Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        if (context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<IAllowAnonymous>() is not null)
        {
            return;
        }
        
        Claim? userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null) throw new InvalidOperationException("User ID claim not found");

        string userId = userIdClaim.Value;

        DatabaseContext database = context.HttpContext.Resolve<DatabaseContext>();
        UserModel? user = await database.Users
            .Include(u => u.Roles)
            .Include(u => u.Organization)
            .SingleOrDefaultAsync(u => u.AuthId == userId, ct);

        if (user is null) user = await SyncUserFromKeycloak(context.HttpContext, database, userId);

        context.HttpContext.Items["User"] = user;
    }

    private async Task<UserModel> SyncUserFromKeycloak(HttpContext context, DatabaseContext database, string userId)
    {
        var result = await GetUserFromDatabase(context, database, userId);

        if (result is null)
        {
            result = await CreateInDatabase(context, database, userId);
        }

        return result;
    }
    
    private async Task<UserModel> CreateInDatabase(HttpContext context, DatabaseContext database, string userId)
    {
        KeycloakConfiguration keycloakConfiguration = context.Resolve<IOptionsMonitor<Configuration>>().CurrentValue.Keycloak;
        var keycloakUser = await LoadUserFromKeycloakViaId(context, keycloakConfiguration.Realm, userId);
        
        UserModel newUser = new UserModel
        {
            Id = UserId.New(),
            AuthId = keycloakUser.Id,
            Email = keycloakUser.Email,
            Firstname = keycloakUser.FirstName,
            Lastname = keycloakUser.LastName,
            Roles = database.Roles.Where(r => keycloakUser.RealmRoles.Contains(r.Name)).ToList(),
            Organization = database.Organizations.FirstOrDefault(x => x.Id == OrganizationId.Empty) ?? new OrganizationModel()
        };

        await database.Users.AddAsync(newUser);
        await database.SaveChangesAsync();

        return newUser;
    }

    private async Task<UserModel?> GetUserFromDatabase(HttpContext context, DatabaseContext database, string userId)
    {
        KeycloakConfiguration keycloakConfiguration = context.Resolve<IOptionsMonitor<Configuration>>().CurrentValue.Keycloak;
        UserRepresentation user = await LoadUserFromKeycloakViaId(context, keycloakConfiguration.Realm, userId);
        UserModel? databaseUser = await database.Users
            .Include(u => u.Roles)
            .Include(u => u.Organization)
            .Where(x => x.Email.ToLower() == user.Email.ToLower()).SingleOrDefaultAsync();

        if (databaseUser is null)
        {
            _logger.Fatal("Could not get User from Database: ClaimUserId: {ClaimUserId}, Email: {Email}", userId, user.Email);
            return null;
        }

        databaseUser.AuthId = userId;

        await database.SaveChangesAsync();

        return databaseUser;
    }

    private async Task<UserRepresentation> LoadUserFromKeycloakViaId(HttpContext context, string realm, string id)
    {
        using AuthenticationHttpClient httpClient = context.Resolve<IKeycloakClientFactory>().CreateClient();
        using UsersApi userApi = ApiClientFactory.Create<UsersApi>(httpClient);

        return await userApi.GetUsersByUserIdAsync(realm, id);
    }
}