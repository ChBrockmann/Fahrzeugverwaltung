namespace Contracts;

public sealed record NotifyUserEvent
{
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}