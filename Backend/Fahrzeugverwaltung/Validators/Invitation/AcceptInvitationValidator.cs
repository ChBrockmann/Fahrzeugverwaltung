using DataAccess.InvitationService;
using Fahrzeugverwaltung.Provider.DateTimeProvider;
using FluentValidation;
using Model.Invitation.Requests;

namespace Fahrzeugverwaltung.Validators.Invitation;

public class AcceptInvitationValidator : Validator<AcceptInvitationRequest>
{
    public AcceptInvitationValidator()
    {
        RuleFor(x => x.Token)
            .MustAsync(async (x, ct) => await CheckIfTokenIsValid(x, ct))
            .WithMessage("Token is invalid");
    }

    private async Task<bool> CheckIfTokenIsValid(string token, CancellationToken ct)
    {
        using var scope = CreateScope();
        var invitationService = scope.Resolve<IInvitationService>();
        var dateTimeProvider = scope.Resolve<IDateTimeProvider>();
        
        var invitation = await invitationService.GetByToken(token);

        if (invitation is null)
            return false;

        return invitation.AcceptedBy is null && invitation.ExpiresAt >= dateTimeProvider.Now;
    }
}