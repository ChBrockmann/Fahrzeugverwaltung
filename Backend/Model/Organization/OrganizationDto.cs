namespace Model.Organization;

public record OrganizationDto
{
    public OrganizationId Id { get; set; } = OrganizationId.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}