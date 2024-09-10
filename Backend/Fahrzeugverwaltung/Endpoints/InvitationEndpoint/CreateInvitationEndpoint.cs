using System.Security.Claims;
using DataAccess.InvitationService;
using DataAccess.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Model;
using Model.Configuration;
using Model.Invitation;
using Model.Invitation.Requests;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.InvitationEndpoint;

public class CreateInvitationEndpoint : Endpoint<CreateInvitationRequest, EmptyResponse>
{
    private readonly IInvitationService _invitationService;
    private readonly IUserService _userService;
    private readonly ILogger _logger;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    
    public CreateInvitationEndpoint(IInvitationService invitationService, ILogger logger, IUserService userService, RoleManager<IdentityRole<Guid>> roleManager, IOptionsMonitor<Configuration> optionsMonitor)
    {
        _invitationService = invitationService;
        _logger = logger;
        _userService = userService;
        _roleManager = roleManager;
        _optionsMonitor = optionsMonitor;
    }

    public override void Configure()
    {
        Post("invitation");
        Roles(Security.AdminRoleName);
    }

    public override async Task HandleAsync(CreateInvitationRequest req, CancellationToken ct)
    {
        string? claimUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (claimUserId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Guid userId = Guid.Parse(claimUserId);
        UserModel? requestingUser = await _userService.Get(userId);
        
        _logger.Information("User {UserId} is creating {Count} invitations", userId, req.Count);

        var roles = await GetRolesFromRequest(req);

        for (int i = 0; i < req.Count; i++)
        {
            InvitationModel invitationModel = new()
            {
                Id = InvitationId.New(),
                CreatedAt = DateTime.Now,
                CreatedBy = requestingUser,
                ExpiresAt = new DateTime(req.ExpiresAfterDay.Year, req.ExpiresAfterDay.Month, req.ExpiresAfterDay.Day, 0, 0, 0),
                Roles = roles,
                Token = await GenerateUniqueToken()
            };

            await _invitationService.Create(invitationModel);
        }
    }
    
    private async Task<List<IdentityRole<Guid>>> GetRolesFromRequest(CreateInvitationRequest req)
    {
        List<IdentityRole<Guid>> roles = new();
        foreach (string roleName in req.Roles)
        {
            IdentityRole<Guid>? role = await _roleManager.FindByNameAsync(roleName);
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