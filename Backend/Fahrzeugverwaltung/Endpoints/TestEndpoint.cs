using System.Security.Claims;
using DataAccess;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints;

public class TestEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private DatabaseContext _database;
    
    public TestEndpoint(ILogger logger, DatabaseContext database)
    {
        _logger = logger;
        _database = database;
    }

    public override void Configure()
    {
        Get("test");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        // _logger.Information("User {User}",User.ToString());
        foreach (Claim claim in User.Claims)
        {
            _logger.Information("Claim {ClaimValueType} {ClaimType} {ClaimValue} {0} {1}", claim.ValueType, claim.Type, claim.Value, claim.Subject, claim.Properties);
        }
        _logger.Information("TestEndpoint was called");
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