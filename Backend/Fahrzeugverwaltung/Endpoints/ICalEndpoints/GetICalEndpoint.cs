using DataAccess;
using DataAccess.ReservationService;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using Microsoft.EntityFrameworkCore;
using Model.Reservation;

namespace Fahrzeugverwaltung.Endpoints.ICalEndpoints;

public class GetICalEndpoint : Endpoint<EmptyRequest, EmptyResponse>
{
    private readonly IReservationService _reservationService;

    public GetICalEndpoint(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    public override void Configure()
    {
        Get("ical");
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var calendar = new Calendar()
        {
            Name = "Fahrzeugverwaltung"
        };

        var allReservations = await _reservationService.Get();

        foreach (ReservationModel reservationModel in allReservations)
        {
            var iCalEvent = new CalendarEvent()
            {
                Uid = reservationModel.Id.ToString(),
                
                Summary = $"Reservierung von {reservationModel.VehicleReserved.Name}",
                Description = $"Fahrzeug {reservationModel.VehicleReserved.Name} reserviert für {reservationModel.ReservationMadeByUser.Organization} " +
                              $"von {reservationModel.ReservationMadeByUser.Firstname} {reservationModel.ReservationMadeByUser.Lastname}." +
                              $"Reservierung erstellt am {reservationModel.ReservationCreated:d.M.yyyy}",
                Start = ConvertDate(reservationModel.StartDateInclusive),
                End = ConvertDate(reservationModel.EndDateInclusive),
                IsAllDay = true
            };

            calendar.Events.Add(iCalEvent);
        }

        CalendarSerializer iCalSerializer = new();
        string result = iCalSerializer.SerializeToString(calendar);

        await using var stream = GenerateStreamFromString(result);
        await SendStreamAsync(stream, "reservations.ics", contentType: "text/calendar", cancellation: ct);
    }

    private CalDateTime ConvertDate(DateOnly date)
    {
        return new CalDateTime(date.Year, date.Month, date.Day);
    }

    private Stream GenerateStreamFromString(string s)
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);
        writer.Write(s);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}