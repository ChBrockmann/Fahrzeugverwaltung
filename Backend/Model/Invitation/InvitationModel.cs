using Model.Roles;
using Model.User;
using StronglyTypedIds;

namespace Model.Invitation;

[StronglyTypedId(Template.Guid)]
public partial struct InvitationId { }

public record InvitationModel : IDatabaseId<InvitationId>
{
    public string Token { get; set; } = string.Empty;

    public UserModel? CreatedBy { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public UserModel? AcceptedBy { get; set; }
    public DateTime? AcceptedAt { get; set; }

    public List<Role> Roles { get; set; } = new();

    public DateTime ExpiresAt { get; set; }
    public InvitationId Id { get; set; } = InvitationId.Empty;
}