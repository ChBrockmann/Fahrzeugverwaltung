using System.ComponentModel.DataAnnotations;
using DataAccess;
using DataAccess.OrganizationService;
using Model;
using Model.Organization;
using Model.Organization.Responses;

namespace Fahrzeugverwaltung.Endpoints.OrganizationEndpoint;

public record GetOrganizationAdminResponse
{
    [Required]
    public List<OrganizationAdminResponse> Organizations { get; set; } = new();
}

public class GetOrganizationAdminEndpoint : BaseEndpoint<EmptyRequest, GetOrganizationAdminResponse>
{
    private readonly IMapper _mapper;
    private readonly IOrganizationService _organizationService;
    public GetOrganizationAdminEndpoint(IMapper mapper, IOrganizationService organizationService)
    {
        _mapper = mapper;
        _organizationService = organizationService;
    }

    public override void Configure()
    {
        Get("organization/admin");
        Roles(SecurityConfiguration.AdminRoleName, SecurityConfiguration.OrganizationAdminRoleName);
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var organizations = (await _organizationService.Get()).ToList();
        
        var response = _mapper.Map<List<OrganizationAdminResponse>>(organizations);
        
        foreach(var organization in response)
        {
            organization.IsAdmin = organizations
                .FirstOrDefault(x => x.Id == organization.Id)?.Admins
                    .Any(x => x.Id == UserFromContext.Id) ?? false;
        }

        await SendOkAsync(new GetOrganizationAdminResponse()
        {
            Organizations = response
        }, ct);
    }
}