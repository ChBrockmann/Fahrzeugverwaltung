﻿using System.Security.Claims;
using DataAccess.ReservationService;
using DataAccess.ReservationStatusService;
using DataAccess.UserService;
using Model;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.ReservationStatus;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.ReservationStatusEndpoint;

public class CreateReservationStatusEndpoint : Endpoint<AddStatusToReservationRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IReservationService _reservationService;
    private readonly IReservationStatusService _reservationStatusService;
    private readonly IUserService _userService;

    public CreateReservationStatusEndpoint(IReservationStatusService reservationStatusService, ILogger logger, IReservationService reservationService, IUserService userService)
    {
        _reservationStatusService = reservationStatusService;
        _logger = logger;
        _reservationService = reservationService;
        _userService = userService;
    }

    public override void Configure()
    {
        Post("reservation/{ReservationId}/status");
        Roles(Security.AdminRoleName);
    }

    public override async Task HandleAsync(AddStatusToReservationRequest req, CancellationToken ct)
    {
        ReservationModel? reservation = await _reservationService.Get(req.ReservationId);
        if (reservation is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        string? claimUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (claimUserId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        Guid userId = Guid.Parse(claimUserId);
        UserModel? requestingUser = await _userService.Get(userId);

        if (requestingUser is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        ReservationStatusModel status = new()
        {
            Reservation = reservation,
            StatusReason = req.Reason,
            Status = req.Status,
            StatusChanged = DateTime.Now,
            StatusChangedByUser = requestingUser
        };
        await _reservationStatusService.AddStatusToReservationAsync(status, ct);

        await SendOkAsync(ct);
    }
}