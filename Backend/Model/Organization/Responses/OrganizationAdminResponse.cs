namespace Model.Organization.Responses;

public record OrganizationAdminResponse
{
    public OrganizationId Id { get; set; } = OrganizationId.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
    
    public bool IsAdmin { get; set; } 
}