using Model.User;
using StronglyTypedIds;

namespace Model.Organization;

[StronglyTypedId(Template.Guid)]
public partial struct OrganizationId { }

public record OrganizationModel : IDatabaseId<OrganizationId>
{
    public OrganizationId Id { get; set; } = OrganizationId.Empty;

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<UserModel> Users { get; set; } = new();

    public List<UserModel> Admins { get; set; } = new();
}