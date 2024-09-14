using Contracts.Mailing;
using DataAccess;
using MailKit.Net.Smtp;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using Model.Configuration;
using Model.Reservation;
using Model.ReservationStatus;
using Model.User;
using Serilog;

namespace Mailing.SendReservationStatusChangedMailConsumer;

public class SendReservationStatusChangedMailConsumer : IConsumer<SendReservationStatusChangedMail>
{
    private readonly ILogger _logger;
    private readonly DatabaseContext _database;
    private readonly IOptionsMonitor<Configuration> _configuration;

    public SendReservationStatusChangedMailConsumer(ILogger logger, DatabaseContext database, IOptionsMonitor<Configuration> configuration)
    {
        _logger = logger;
        _database = database;
        _configuration = configuration;
    }

    public async Task Consume(ConsumeContext<SendReservationStatusChangedMail> context)
    {
        if (!_configuration.CurrentValue.Mailing.MailTypes.ReservationRequestStatusChange)
        {
            _logger.Information("Reservation status changed mail is disabled in the configuration");
            return;
        }

        ReservationModel? reservation = await _database
            .ReservationModels
            .Include(x => x.ReservationMadeByUser)
            .ThenInclude(x => x.Organization)
            .ThenInclude(x => x.Admins)
            .Include(reservationModel => reservationModel.VehicleReserved).Include(reservationModel => reservationModel.ReservationStatusChanges)
            .FirstOrDefaultAsync(x => x.Id == context.Message.ReservationId);

        if (reservation is null)
        {
            _logger.Error("Could not send email for ReserationId {ReservationId} because the reservation was not found", context.Message.ReservationId);
            return;
        }

        SmtpSettingsConfiguration smtpConfig = _configuration.CurrentValue.Mailing.SmtpSettings;
        UserModel user = reservation.ReservationMadeByUser;
        ReservationStatusModel latestStatusChange = reservation.ReservationStatusChanges.OrderByDescending(x => x.StatusChanged).First();

        MimeMessage message = new();

        message.From.Add(new MailboxAddress(smtpConfig.SenderName, smtpConfig.SenderEmail));
        message.To.Add(new MailboxAddress($"{user.Firstname} {user.Lastname}", user.Email));
        message.Subject = "Statusänderung einer Reservierungsanfrage";

        message.Body = new TextPart("plain")
        {
            Text = $"""
                    Hallo {user.Firstname} {user.Lastname},

                    deine Reservierungsanfrage wurde bearbeitet

                    Reservierungsanfrage:
                    Fahrzeug: {reservation.VehicleReserved.Name}
                    Begründung: {reservation.Reason}
                    Zeitraum: {reservation.StartDateInclusive.ToString("dd.MM.yyyy")} - {reservation.EndDateInclusive.ToString("dd.MM.yyyy")}

                    Statusänderung:
                    Status: {latestStatusChange.Status}
                    Begründung: {latestStatusChange.StatusReason}
                    """
        };

        using SmtpClient client = new();
        await client.ConnectAsync(smtpConfig.Host, smtpConfig.Port, smtpConfig.UseSsl);

        if (!string.IsNullOrEmpty(smtpConfig.Username) && !string.IsNullOrEmpty(smtpConfig.Password)) await client.AuthenticateAsync(smtpConfig.Username, smtpConfig.Password);

        await client.SendAsync(message);
        _logger.Information("Send email for reservation status change with ID {ReservationId} to {RecipientCount} Recipient", context.Message.ReservationId, message.To.Count);
        await client.DisconnectAsync(true);
    }
}