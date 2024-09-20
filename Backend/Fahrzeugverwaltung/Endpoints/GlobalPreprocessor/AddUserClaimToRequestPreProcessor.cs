using System.Security.Claims;

namespace Fahrzeugverwaltung.Endpoints.GlobalPreprocessor;

public class AddUserClaimToRequestPreProcessor : IGlobalPreProcessor
{
    public async Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
    {
        Guid userId = Guid.Parse("08dcd7d6-8ff0-42b6-8438-0fae864010c7");
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