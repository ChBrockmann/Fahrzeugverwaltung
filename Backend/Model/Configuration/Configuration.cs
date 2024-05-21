namespace Model.Configuration;

public record Configuration
{
    public string DatabaseConnectionString { get; set; } = string.Empty;

    public int BearerTokenExpirationInMinutes { get; set; }

    public bool AuthenticationEnabled { get; set; }
}