namespace Fahrzeugverwaltung.Endpoints.GlobalPreprocessor;

// public class AddUserClaimToRequestPreProcessor : IGlobalPreProcessor
// {
//
//     public async Task PreProcessAsync(IPreProcessorContext context, CancellationToken ct)
//     {
//         Guid userId = Guid.Parse("4098D24F-0A17-4BAA-B895-B5C6E3926DE7");
//         
//         context.HttpContext.User.AddIdentity(new ClaimsIdentity([new Claim(ClaimTypes.NameIdentifier, userId.ToString())]));
//     }
// }