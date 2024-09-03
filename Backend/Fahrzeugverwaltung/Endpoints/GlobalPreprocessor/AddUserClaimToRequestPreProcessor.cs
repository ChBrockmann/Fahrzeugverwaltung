using System.Security.Claims;

namespace Fahrzeugverwaltung.Endpoints.GlobalPreprocessor;

public class AddUserClaimToRequestPreProcessor : IGlobalPreProcessor
{
    public async Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        Guid userId = Guid.Parse("08dccc2e-0381-456b-85ec-701fa598af33");
        string[] userRoles =
        [
            "Admin"
        ];

        context.HttpContext.User.AddIdentity(new ClaimsIdentity(
        [
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Role, userRoles.Aggregate((x, y) => x + y))
        ]));
        // context.HttpContext.User.AddIdentity(new ClaimsIdentity());
    }
}