using DataAccess.InvitationService;
using DataAccess.OrganizationService;
using DataAccess.UserService;
using Model.Invitation;
using Model.Invitation.Requests;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class AcceptInvitationEndpoint : Endpoint<AcceptInvitationRequest, EmptyResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly ILogger _logger;
    private readonly IValidator<AcceptInvitationRequest> _validator;
    private readonly IUserService _userService;
    private readonly IOrganizationService _organizationService;

    public AcceptInvitationEndpoint(ILogger logger, IInvitationService invitationService,
        IValidator<AcceptInvitationRequest> validator, IUserService userService, IOrganizationService organizationService)
    {
        _logger = logger;
        _invitationService = invitationService;
        _validator = validator;
        _userService = userService;
        _organizationService = organizationService;
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

        UserModel result = await _userService.Create(new UserModel
        {
            Id = UserId.New(),
            Firstname = req.Firstname,
            Lastname = req.Lastname,
            Email = req.Email,
            Organization = await _organizationService.GetOrCreate(req.Organization)
        });

        _logger.Information("User created successfully in database");


        UserModel dbUser = await _userService.GetUserByEmail(req.Email) ?? throw new ArgumentNullException();

        await _userService.SetRolesOfUser(result.Id, invitation.Roles);

        await _invitationService.SetAcceptedByUser(invitation.Id, dbUser);

        await SendOkAsync(ct);
    }
}