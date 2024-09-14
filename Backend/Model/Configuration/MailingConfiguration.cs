namespace Model.Configuration;

public sealed record MailingConfiguration
{
    public const string SectionName = "Mailing";

    public SmtpSettingsConfiguration SmtpSettings { get; set; } = new();

    public MailTypes MailTypes { get; set; } = new();
}

public sealed record MailTypes
{
    public const string SectionName = "MailTypes";

    public bool NewReservationRequest { get; set; }
    public bool ReservationRequestStatusChange { get; set; }
}