using DataAccess.OrganizationService;
using Model.Organization;
using Model.Organization.Responses;

namespace Fahrzeugverwaltung.Endpoints.OrganizationEndpoint;

public class GetAllOrganizationsAnonmyousEndpoint : Endpoint<EmptyRequest, GetAllOrganizationsAnonymousResponse>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;

    public GetAllOrganizationsAnonmyousEndpoint(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("organization/anonymous");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        IEnumerable<OrganizationModel> organizations = await _organizationService.Get();

        await SendOkAsync(new GetAllOrganizationsAnonymousResponse
        {
            Organizations = _mapper.Map<List<OrganizationBasicResponse>>(organizations)
        }, ct);
    }
}