using System.ComponentModel.DataAnnotations;

namespace Model.Invitation.Requests;

public record AcceptInvitationRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public string Firstname { get; set; } = string.Empty;
    
    [Required]
    public string Lastname { get; set; } = string.Empty;
    
    [Required]
    public string Organization { get; set; } = string.Empty;

    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;
}