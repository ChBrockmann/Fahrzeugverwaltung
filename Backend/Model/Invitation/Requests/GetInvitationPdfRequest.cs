using System.ComponentModel.DataAnnotations;

namespace Model.Invitation.Requests;

public class GetInvitationPdfRequest
{
    [Required]
    public InivitationId Id { get; set; } = InivitationId.Empty;
    
    [Required]
    public string BaseUrl { get; set; } = string.Empty;
}