using DataAccess.InvitationService;
using DataAccess.RoleService;
using DataAccess.UserService;
using Microsoft.Extensions.Options;
using Model;
using Model.Invitation;
using Model.Invitation.Requests;
using Model.Roles;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class CreateInvitationEndpoint : BaseEndpoint<CreateInvitationRequest, EmptyResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly IUserService _userService;
    private readonly ILogger _logger;
    private readonly IRoleService _roleService;
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;

    public CreateInvitationEndpoint(IInvitationService invitationService, ILogger logger, IUserService userService, IOptionsMonitor<Configuration> optionsMonitor, IRoleService roleService)
    {
        _invitationService = invitationService;
        _logger = logger;
        _userService = userService;
        _optionsMonitor = optionsMonitor;
        _roleService = roleService;
    }

    public override void Configure()
    {
        Post("invitation");
        Roles(SecurityConfiguration.AdminRoleName, SecurityConfiguration.OrganizationAdminRoleName);
    }

    public override async Task HandleAsync(CreateInvitationRequest req, CancellationToken ct)
    {
        UserId userId = UserFromContext.Id;
        UserModel? requestingUser = await _userService.Get(userId);
        
        _logger.Information("User {UserId} is creating {Count} invitations", userId, req.Count);

        var roles = await GetRolesFromRequest(req);

        for (int i = 0; i < req.Count; i++)
        {
            string? note = req.Notes.ElementAtOrDefault(i);
            InvitationModel invitationModel = new()
            {
                Id = InvitationId.New(),
                CreatedAt = DateTime.Now,
                CreatedBy = requestingUser,
                ExpiresAt = new DateTime(req.ExpiresAfterDay.Year, req.ExpiresAfterDay.Month, req.ExpiresAfterDay.Day, 0, 0, 0),
                Roles = roles,
                Note = note,
                Token = await GenerateUniqueToken()
            };

            await _invitationService.Create(invitationModel);
        }
    }

    private async Task<List<Role>> GetRolesFromRequest(CreateInvitationRequest req)
    {
        List<Role> roles = new();
        foreach (string roleName in req.Roles)
        {
            Role? role = await _roleService.Get(roleName);
            if (role is null)
            {
                _logger.Warning("Role {RoleName} not found", roleName);
                continue;
            }

            roles.Add(role);
        }

        return roles;
    }

    private string RandomString()
    {
        Random random = new();
        var tokenGenerationOptions = _optionsMonitor.CurrentValue.Invitation.TokenGenerationOptions;
        int length = tokenGenerationOptions.Length;
        length = (length < 1) ? 4 : length;
        
        string chars = tokenGenerationOptions.Uppercase ? "ABCDEFGHIJKLMNOPQRSTUVWXYZ" : string.Empty;
        chars += tokenGenerationOptions.Lowercase ? "abcdefghijklmnopqrstuvwxyz" : string.Empty;
        chars += tokenGenerationOptions.Numbers || chars.Length == 0 ? "0123456789" : string.Empty;
        
        _logger.Information("Generating random string with length {Length}. Uppercase: {Uppercase}, lowercase: {lowercase}, numbers: {numbers}", length, tokenGenerationOptions.Uppercase, tokenGenerationOptions.Lowercase, tokenGenerationOptions.Numbers);
        
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
    
    private async Task<string> GenerateUniqueToken()
    {
        string token;
        
        do
        {
            token = RandomString();
        } while (await _invitationService.GetByToken(token) is not null);

        return token;
    }
}