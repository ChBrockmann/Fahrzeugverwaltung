using Model.User;

namespace Model.Invitation;

public record InvitationModelDto
{
    public InvitationId Id { get; set; } = InvitationId.Empty;

    public string Token { get; set; } = string.Empty;
    public string? Note { get; set; }

    public UserDto? CreatedBy { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public UserDto? AcceptedBy { get; set; }
    public DateTime? AcceptedAt { get; set; }

    public List<string> Roles { get; set; } = new();

    public DateTime ExpiresAt { get; set; }
}