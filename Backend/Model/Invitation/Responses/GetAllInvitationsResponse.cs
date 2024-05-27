namespace Model.Invitation.Responses;

public record GetAllInvitationsResponse
{
    public List<InvitationModelDto> Invitations { get; set; } = new();
}