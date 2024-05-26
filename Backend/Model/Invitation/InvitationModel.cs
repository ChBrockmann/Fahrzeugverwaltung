using Microsoft.AspNetCore.Identity;
using Model.User;
using StronglyTypedIds;

namespace Model.Invitation;

[StronglyTypedId(Template.Guid)]
public partial struct InivitationId { }

public record InvitationModel : IDatabaseId<InivitationId>
{
    public string Token { get; set; } = string.Empty;

    public UserModel CreatedBy { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public UserModel? AcceptedBy { get; set; }
    public DateTime? AcceptedAt { get; set; }

    public List<IdentityRole<Guid>> Roles { get; set; } = new();

    public DateTime ExpiresAt { get; set; }
    public InivitationId Id { get; set; } = InivitationId.Empty;
}