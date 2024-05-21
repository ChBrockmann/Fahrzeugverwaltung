using System.Security.Claims;
using DataAccess;
using DataAccess.UserService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints;

public class TestEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly UserManager<UserModel> _userManager;
    private readonly IUserService _userService;
    private DatabaseContext _database;

    public TestEndpoint(ILogger logger, DatabaseContext database, RoleManager<IdentityRole<Guid>> roleManager, UserManager<UserModel> userManager, IUserService userService)
    {
        _logger = logger;
        _database = database;
        _roleManager = roleManager;
        _userManager = userManager;
        _userService = userService;
    }

    public override void Configure()
    {
        Get("test");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        _logger.Information("Test Endpoint Called!");
        IEnumerable<UserModel> user = await _userService.Get();

        // var dbUser = _database.UserRoles.Add(new IdentityUserRole<Guid>()
        // {
        //     RoleId = Guid.Parse("08dc773c-6496-40ec-839f-755d35d819a2"),
        //     UserId = Guid.Parse("4098D24F-0A17-4BAA-B895-B5C6E3926DE7")
        // });
        // await _database.SaveChangesAsync(ct);
        //
        // await _userManager.RemoveFromRoleAsync(user.First(x => x.Firstname.Contains("Jeff")), "Admin");
        foreach (Claim claim in User.Claims) _logger.Information("Claim {ClaimValueType} {ClaimType} {ClaimValue} {0} {1}", claim.ValueType, claim.Type, claim.Value, claim.Subject, claim.Properties);

        List<IdentityRole<Guid>> roles = await _roleManager.Roles.ToListAsync(ct);
        foreach (IdentityRole<Guid> role in roles) _logger.Information("Role {RoleName} Guid: {Guid} UserIsInRole: {UserIsInRole}", role.Name, role.Id, User.IsInRole(role.Name!));


        // var claims = ClaimsPrincipal.Current?.Identities.First().Claims.ToList();
        // _logger.Information(claims?.Select(x => x.ToString()).ToString() ?? string.Empty);
        // foreach (var user in _database.Users)
        // {
        //     _database.UserClaims.Add(new()
        //     {
        //         Id = new Random().Next(0, 999999999),
        //         UserId = user.Id,
        //         ClaimType = "TestClaim",
        //         ClaimValue = user.Id.ToString()
        //     });
        //     await _database.SaveChangesAsync(ct);
        // }
        await SendOkAsync(ct);
    }
}