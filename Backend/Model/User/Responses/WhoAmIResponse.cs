using System.ComponentModel.DataAnnotations;

namespace Model.User.Responses;

public record WhoAmIResponse
{
    [Required]
    public UserDto User { get; set; } = new();

    [Required]
    public List<string> Roles { get; set; } = new();
}