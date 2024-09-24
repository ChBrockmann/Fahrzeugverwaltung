using DataAccess.InvitationService;
using DataAccess.Provider.DateTimeProvider;
using Fahrzeugverwaltung.Keycloak;
using FS.Keycloak.RestApiClient.Api;
using FS.Keycloak.RestApiClient.Authentication.Client;
using FS.Keycloak.RestApiClient.Model;
using Microsoft.Extensions.Options;
using Model.Invitation.Requests;
using ApiClientFactory = FS.Keycloak.RestApiClient.ClientFactory.ApiClientFactory;

namespace Fahrzeugverwaltung.Validators.Invitation;

public class AcceptInvitationValidator : AbstractValidator<AcceptInvitationRequest>
{
    private readonly IInvitationService _invitationService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    private readonly IKeycloakClientFactory _keycloakClientFactory;

    public AcceptInvitationValidator(IDateTimeProvider dateTimeProvider, IInvitationService invitationService, IKeycloakClientFactory keycloakClientFactory, IOptionsMonitor<Configuration> optionsMonitor)
    {
        _dateTimeProvider = dateTimeProvider;
        _invitationService = invitationService;
        _keycloakClientFactory = keycloakClientFactory;
        _optionsMonitor = optionsMonitor;
        RuleFor(x => x.Token)
            .MustAsync(async (x, ct) => await CheckIfTokenIsValid(x, ct))
            .WithMessage("Token is invalid");
        RuleFor(x => x.Email)
            .MustAsync(async (x, ct) => await CheckIfEmailIsUnique(x, ct))
            .WithMessage("Email is already in use");
    }

    private async Task<bool> CheckIfEmailIsUnique(string email, CancellationToken ct)
    {
        string realm = _optionsMonitor.CurrentValue.Keycloak.Realm;

        using AuthenticationHttpClient httpClient = _keycloakClientFactory.CreateClient();
        using UsersApi userApi = ApiClientFactory.Create<UsersApi>(httpClient);

        List<UserRepresentation>? user = await userApi.GetUsersAsync(realm, email: email, cancellationToken: ct);

        return user is null || user.Count == 0;
    }

    private async Task<bool> CheckIfTokenIsValid(string token, CancellationToken ct)
    {
        var invitation = await _invitationService.GetByToken(token);

        if (invitation is null)
            return false;

        return invitation.AcceptedBy is null && invitation.ExpiresAt >= _dateTimeProvider.Now;
    }
}