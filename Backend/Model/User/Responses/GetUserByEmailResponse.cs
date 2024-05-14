namespace Model.User.Responses;

public record GetUserByEmailResponse
{
    public UserDto? User { get; set; }
}