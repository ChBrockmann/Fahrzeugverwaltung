namespace Fahrzeugverwaltung.Endpoints;

public class UnAuthorizedEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
    public override void Configure()
    {
        Get("unauthorized");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        await SendUnauthorizedAsync(ct);
    }
}