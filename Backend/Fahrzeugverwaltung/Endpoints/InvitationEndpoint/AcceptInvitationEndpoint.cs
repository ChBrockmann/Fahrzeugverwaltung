using DataAccess.InvitationService;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Model.Invitation;
using Model.Invitation.Requests;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class AcceptInvitationEndpoint : Endpoint<AcceptInvitationRequest, EmptyResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly ILogger _logger;
    private UserManager<UserModel> _userManager;
    private readonly IValidator<AcceptInvitationRequest> _validator;

    public AcceptInvitationEndpoint(ILogger logger, IInvitationService invitationService, 
        UserManager<UserModel> userManager, IValidator<AcceptInvitationRequest> validator)
    {
        _logger = logger;
        _invitationService = invitationService;
        _userManager = userManager;
        _validator = validator;
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

        var result = await _userManager.CreateAsync(new UserModel()
        {
            Firstname = req.Firstname,
            Lastname = req.Lastname,
            //TODO CB - 2024-09-13 fix this
            // Organization = req.Organization,
            Email = req.Email,
            UserName = req.Email
        }, req.Password);

        _logger.Information(result.Succeeded ? "User created successfully" : "User creation failed");
        if (result.Succeeded)
        {
            var dbUser = await _userManager.FindByEmailAsync(req.Email) ?? throw new ArgumentNullException();

            await _userManager.AddToRolesAsync(dbUser, invitation.Roles.Where(x => x.Name is not null).Select(x => x.Name).ToList()!);
            
            await _invitationService.SetAcceptedByUser(invitation.Id, dbUser);
        }
        else
        {
            var error = result.Errors.First();
            _logger.Information("User creation failed with error {Code}: {Description}", error.Code, error.Description);
            ThrowError(new ValidationFailure(error.Code, error.Description));
        }
    }
}