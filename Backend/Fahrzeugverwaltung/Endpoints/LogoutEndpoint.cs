using Microsoft.AspNetCore.Identity;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints;

public class LogoutEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
    private readonly SignInManager<UserModel> _signInManager;
    
    public LogoutEndpoint(SignInManager<UserModel> signInManager)
    {
        _signInManager = signInManager;
    }

    public override void Configure()
    {
        Post("identity/logout");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        await _signInManager.SignOutAsync();
        await SendOkAsync(ct);
    }
}