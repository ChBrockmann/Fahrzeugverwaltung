namespace Model.Configuration;

public record Configuration
{
    public string DatabaseConnectionString { get; set; } = string.Empty;
}