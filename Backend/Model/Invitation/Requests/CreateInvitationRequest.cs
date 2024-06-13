using System.ComponentModel.DataAnnotations;

namespace Model.Invitation.Requests;

public class CreateInvitationRequest
{
    [Required]
    public int Count { get; set; } = 0;
    
    [Required]
    public List<string> Roles { get; set; } = new();
    
    [Required]
    public DateTime ExpiresAfterDay { get; set; }
}