using System.Net.Mail;
using Contracts.Notification;
using MassTransit;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Reservation;
using Model.User;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Consumers;

public class NewReservationCreatedEventConsumer: IConsumer<NewReservationCreatedEvent>
{
    private readonly ILogger _logger;
    private readonly IOptionsMonitor<Configuration> _configuration;

    public NewReservationCreatedEventConsumer(ILogger logger, IOptionsMonitor<Configuration> configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<NewReservationCreatedEvent> context)
    {
        if (!_configuration.CurrentValue.Mailing.MailTypes.NewReservationRequest)
        {
            _logger.Information("New reservation mail is disabled in the configuration");
            return;
        }
        
        _logger.Information("Sending E-Mail for new reservation with ID {ReservationId}", context.Message.ReservationId);

        ReservationModel? reservation = await _database
            .ReservationModels
            .Include(x => x.ReservationMadeByUser)
            .ThenInclude(x => x.Organization)
            .ThenInclude(x => x.Admins)
            .Include(reservationModel => reservationModel.VehicleReserved)
            .FirstOrDefaultAsync(x => x.Id == context.Message.ReservationId);

        if (reservation is null)
        {
            _logger.Error("Could not send email for ReserationId {ReservationId} because the reservation was not found", context.Message.ReservationId);
            return;
        }

        SmtpSettingsConfiguration smtpConfig = _configuration.CurrentValue.Mailing.SmtpSettings;

        MimeMessage message = new();

        message.From.Add(new MailboxAddress(smtpConfig.SenderName, smtpConfig.SenderEmail));

        foreach (UserModel admin in reservation.ReservationMadeByUser.Organization.Admins) message.To.Add(new MailboxAddress($"{admin.Firstname} {admin.Lastname}", admin.Email));

        message.Subject = "Neue Reservierungsanfrage";

        message.Body = new TextPart("plain")
        {
            Text = $"""
                    Hallo,

                    es liegt eine neue Reservierungsanfrage vor:

                    Reservierungsanfrage von: {reservation.ReservationMadeByUser.Firstname} {reservation.ReservationMadeByUser.Lastname}
                    Fahrzeug: {reservation.VehicleReserved.Name}
                    Begründung: {reservation.Reason}
                    Zeitraum: {reservation.StartDateInclusive.ToString("dd.MM.yyyy")} - {reservation.EndDateInclusive.ToString("dd.MM.yyyy")}

                    Die Anfrage wurde am {reservation.ReservationCreated:dd.MM.yyyy HH:mm} erstellt.
                    """
        };

        using SmtpClient client = new();
        await client.ConnectAsync(smtpConfig.Host, smtpConfig.Port, smtpConfig.UseSsl);

        if (!string.IsNullOrEmpty(smtpConfig.Username) && !string.IsNullOrEmpty(smtpConfig.Password)) await client.AuthenticateAsync(smtpConfig.Username, smtpConfig.Password);

        await client.SendAsync(message);
        _logger.Information("Send email for new reservation with ID {ReservationId} to {RecipientCount} Recipient", context.Message.ReservationId, message.To.Count);
        await client.DisconnectAsync(true);
    }
}