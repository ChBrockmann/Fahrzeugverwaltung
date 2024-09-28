namespace Model.Organization.Responses;

public record OrganizationBasicResponse
{
    public OrganizationId Id { get; set; } = OrganizationId.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}