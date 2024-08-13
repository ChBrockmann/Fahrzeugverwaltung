using System.ComponentModel.DataAnnotations;

namespace Model.Invitation.Requests;

public class GetInvitationPdfRequest
{
    [Required]
    public InvitationId Id { get; set; } = InvitationId.Empty;
    
    [Required]
    public string BaseUrl { get; set; } = string.Empty;
}