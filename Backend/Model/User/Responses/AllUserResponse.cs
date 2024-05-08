using System.ComponentModel.DataAnnotations;

namespace Model.User.Responses;

public record AllUserResponse
{
    [Required]
    public List<UserDto> AllUsers { get; set; } = new();
}