using Model.Organization;

namespace Model.User;

public sealed record UserDto
{
    public UserId Id { get; set; } = UserId.Empty;
    public string AuthId { get; set; } = string.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Fullname => Firstname + " " + Lastname;

    public OrganizationDto Organization { get; set; } = new();
}