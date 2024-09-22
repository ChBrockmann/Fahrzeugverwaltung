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
        return await GetUserFromDatabase(context, database, userId);
    }

    private async Task<UserModel> GetUserFromDatabase(HttpContext context, DatabaseContext database, string userId)
    {
        KeycloakConfiguration keycloakConfiguration = context.Resolve<IOptionsMonitor<Configuration>>().CurrentValue.Keycloak;
        UserModel user = await LoadUserFromKeycloakViaId(context, keycloakConfiguration.Realm, userId);
        UserModel? databaseUser = await database.Users
            .Include(u => u.Roles)
            .Include(u => u.Organization)
            .Where(x => x.Email.ToLower() == user.Email.ToLower()).SingleOrDefaultAsync();

        if (databaseUser is null)
        {
            _logger.Fatal("Could not get User from Database: ClaimUserId: {ClaimUserId}, Email: {Email}", userId, user.Email);
            return null!;
        }

        databaseUser.AuthId = userId;

        await database.SaveChangesAsync();

        return databaseUser;
    }

    private async Task<UserModel> LoadUserFromKeycloakViaId(HttpContext context, string realm, string id)
    {
        using AuthenticationHttpClient httpClient = context.Resolve<IKeycloakClientFactory>().CreateClient();
        using UsersApi userApi = ApiClientFactory.Create<UsersApi>(httpClient);

        UserRepresentation result = await userApi.GetUsersByUserIdAsync(realm, id);

        return new UserModel
        {
            AuthId = result.Id,
            Firstname = result.FirstName,
            Lastname = result.LastName,
            Email = result.Email
        };
    }
}