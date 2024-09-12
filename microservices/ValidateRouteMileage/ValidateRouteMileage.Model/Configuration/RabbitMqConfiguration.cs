namespace ValidateRouteMileage.Model.Configuration;

public sealed record RabbitMqConfiguration
{
    public string Host { get; set; } = string.Empty;
    public string VirtualHost { get; set; } = string.Empty;
    public ushort Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}