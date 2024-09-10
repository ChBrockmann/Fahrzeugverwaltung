namespace Model.Organization.Requests;

public record GetAllOrganizationsResponse
{
    public List<OrganizationDto> Organizations { get; set; } = new();
}