using DataAccess.InvitationService;
using DataAccess.Provider.DateTimeProvider;
using FluentValidation;
using Model.Invitation.Requests;

namespace Fahrzeugverwaltung.Validators.Invitation;

public class AcceptInvitationValidator : AbstractValidator<AcceptInvitationRequest>
{
    private readonly IInvitationService _invitationService;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    
    public AcceptInvitationValidator(IDateTimeProvider dateTimeProvider, IInvitationService invitationService)
    {
        _dateTimeProvider = dateTimeProvider;
        _invitationService = invitationService;
        RuleFor(x => x.Token)
            .MustAsync(async (x, ct) => await CheckIfTokenIsValid(x, ct))
            .WithMessage("Token is invalid");
    }

    private async Task<bool> CheckIfTokenIsValid(string token, CancellationToken ct)
    {
        var invitation = await _invitationService.GetByToken(token);

        if (invitation is null)
            return false;

        return invitation.AcceptedBy is null && invitation.ExpiresAt >= _dateTimeProvider.Now;
    }
}