namespace Model.Configuration;

public record Configuration
{
    public string RootOrganizationName { get; set; } = string.Empty;
    public string DatabaseConnectionString { get; set; } = string.Empty;
    
    public bool AuthenticationEnabled { get; set; }

    public ReservationRestrictions ReservationRestrictions { get; set; } = new();
    
    public Invitation Invitation { get; set; } = new();
    public MailingConfiguration Mailing { get; set; } = new();

    public KeycloakConfiguration Keycloak { get; set; } = new();

    public RabbitMqConfiguration RabbitMq { get; set; } = new();
}

public record Invitation
{
    public TokenGenerationOptions TokenGenerationOptions { get; set; } = new();
}

public record TokenGenerationOptions
{
    public int Length { get; set; }
    public bool Numbers { get; set; }
    public bool Uppercase { get; set; }
    public bool Lowercase { get; set; }
}