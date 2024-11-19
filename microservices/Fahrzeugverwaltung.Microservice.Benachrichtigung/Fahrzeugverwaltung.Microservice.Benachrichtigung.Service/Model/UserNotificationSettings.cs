namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;

public class UserNotificationSettings
{
    public Guid UserId { get; set; }
    public bool SendMail { get; set; }
    public bool SendPush { get; set; }
}