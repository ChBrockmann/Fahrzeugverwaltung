using DataAccess.OrganizationService;
using Model;
using Model.Organization;

namespace Fahrzeugverwaltung.Endpoints.OrganizationEndpoint;

public record SetOrganizationAdminRequest
{
    public List<OrganizationId> AdminOrganizationIds { get; set; } = new();
}

public class SetOrganizationAdminEndpoint : BaseEndpoint<SetOrganizationAdminRequest, EmptyResponse>
{
    private readonly IOrganizationService _organizationService;
    public SetOrganizationAdminEndpoint(IOrganizationService organizationService)
    {
        _organizationService = organizationService;
    }

    public override void Configure()
    {
        Post("organization/admin");
        Roles(SecurityConfiguration.AdminRoleName, SecurityConfiguration.OrganizationAdminRoleName);
    }

    public override async Task HandleAsync(SetOrganizationAdminRequest req, CancellationToken ct)
    {
        var allOrganizations = (await _organizationService.Get()).ToList();

        foreach (var organization in allOrganizations)
        {
            if (req.AdminOrganizationIds.Any(x => x == organization.Id))
            {
                await _organizationService.SetOrganizationAdmin(organization.Id, UserFromContext);
            }
            else
            {
                await _organizationService.RemoveOrganizationAdmin(organization.Id, UserFromContext.Id);
            }
        }

        await SendOkAsync(ct);
    }
}