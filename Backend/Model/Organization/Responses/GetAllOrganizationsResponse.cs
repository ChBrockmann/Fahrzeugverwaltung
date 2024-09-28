using System.ComponentModel.DataAnnotations;

namespace Model.Organization.Responses;

public record GetAllOrganizationsAnonymousResponse
{
    public List<OrganizationBasicResponse> Organizations { get; set; } = new();
}

public record GetAllOrganizationsResponse
{
    [Required]
    public List<OrganizationDto> Organizations { get; set; } = new();
}