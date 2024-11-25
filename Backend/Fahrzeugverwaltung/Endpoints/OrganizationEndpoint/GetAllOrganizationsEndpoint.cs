﻿using DataAccess.OrganizationService;
using Model;
using Model.Organization;
using Model.Organization.Responses;

namespace Fahrzeugverwaltung.Endpoints.OrganizationEndpoint;

public class GetAllOrganizationsEndpoint : Endpoint<EmptyRequest, GetAllOrganizationsResponse>
{
    private readonly IOrganizationService _organizationService;
    private readonly IMapper _mapper;
    
    public GetAllOrganizationsEndpoint(IOrganizationService organizationService, IMapper mapper)
    {
        _organizationService = organizationService;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Get("organization");
        Roles(SecurityConfiguration.OrganizationAdminRoleName, SecurityConfiguration.AdminRoleName);
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        IEnumerable<OrganizationModel> organizations = await _organizationService.Get();

        await SendOkAsync(new GetAllOrganizationsResponse
        {
            Organizations = _mapper.Map<List<OrganizationDto>>(organizations)
        }, ct);
    }
}