namespace Model.User;

public sealed record UserDto
{
    public Guid Id { get; set; } = Guid.Empty;

    public string Firstname { get; set; } = string.Empty;

    public string Lastname { get; set; } = string.Empty;

    public string Fullname => Firstname + " " + Lastname;

    public string Organization { get; set; } = string.Empty;
}