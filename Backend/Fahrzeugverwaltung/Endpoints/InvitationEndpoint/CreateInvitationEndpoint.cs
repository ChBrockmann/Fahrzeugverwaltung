using System.Security.Claims;
using DataAccess.InvitationService;
using DataAccess.UserService;
using Microsoft.AspNetCore.Identity;
using Model;
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
    
    public CreateInvitationEndpoint(IInvitationService invitationService, ILogger logger, IUserService userService, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _invitationService = invitationService;
        _logger = logger;
        _userService = userService;
        _roleManager = roleManager;
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
                Id = InivitationId.New(),
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
    
    private static readonly Random Random = new();

    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
    
    private async Task<string> GenerateUniqueToken()
    {
        string token;
        
        do
        {
            token = RandomString(8);
        } while (await _invitationService.GetByToken(token) is not null);

        return token;
    }
}