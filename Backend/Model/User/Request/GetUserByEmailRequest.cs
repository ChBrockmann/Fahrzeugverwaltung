using System.ComponentModel.DataAnnotations;

namespace Model.User.Request;

public record GetUserByEmailRequest
{
    [Required]
    public string Email { get; set; } = string.Empty;
}