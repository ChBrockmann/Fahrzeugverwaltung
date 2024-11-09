﻿using Contracts;
using DataAccess.LogBookEntryService;
using DataAccess.ReservationService;
using DataAccess.UserService;
using MassTransit;
using Model.LogBook;
using Model.LogBook.Requests;
using Model.Reservation;

namespace Fahrzeugverwaltung.Endpoints.LogBookEntryEndpoint;

public class CreateLogBookEntryWithReservationEndpoint : BaseEndpoint<CreateLogBookRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IUserService _userService;
    private readonly ILogBookEntryService _logBookEntryService;
    private readonly IReservationService _reservationService;

    public CreateLogBookEntryWithReservationEndpoint(ILogger logger, IPublishEndpoint publishEndpoint, IUserService userService,
        ILogBookEntryService logBookEntryService, IReservationService reservationService)
    {
        _logger = logger;
        _publishEndpoint = publishEndpoint;
        _userService = userService;
        _logBookEntryService = logBookEntryService;
        _reservationService = reservationService;
    }

    public override void Configure()
    {
        Post("logbookentry/reservation/{ReservationId}");
        AllowFileUploads();
    }

    public override async Task HandleAsync(CreateLogBookRequest req, CancellationToken ct)
    {
        _logger.Information("Creating log book entry");

        using MemoryStream memoryStream = new();
        await req.ImageFile.CopyToAsync(memoryStream, ct);
        byte[] bytes = memoryStream.ToArray();

        ReservationModel? reservation = await _reservationService.Get(req.ReservationId);
        if (reservation is null)
        {
            _logger.Error("Could not find Reservation {ReservationId}", req.ReservationId);
            ThrowError("Reservation not found");
        }

        LogBookEntry logbookEntry = await _logBookEntryService.Create(new LogBookEntry
        {
            AssociatedReservation = reservation,
            AssociatedVehicle = reservation.VehicleReserved,
            Id = LogBookEntryId.New(),
            CreatedAt = DateTime.Now,
            CreatedBy = UserFromContext,
            Description = req.Description
        });

        LogbookEntryCreatedEvent logBookEntryCreated = new()
        {
            LogBookEntryId = logbookEntry.Id,
            ImageAsBase64 = Convert.ToBase64String(bytes),
            ImageType = req.ImageFile.ContentType
        };

        await _publishEndpoint.Publish(logBookEntryCreated, ct);

        await SendOkAsync(ct);
    }
}