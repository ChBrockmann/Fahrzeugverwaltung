using System.ComponentModel.DataAnnotations;
using Model.User;

namespace Model.Organization;

public record OrganizationDto
{
    [Required]
    public OrganizationId Id { get; set; } = OrganizationId.Empty;

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;
    
    [Required]
    public List<UserDto> Users { get; set; } = new();
}