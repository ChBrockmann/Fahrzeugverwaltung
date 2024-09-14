using System.Security.Claims;
using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class DeleteReservationEndpoint : Endpoint<DeleteReservationRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IReservationService _reservationService;

    public DeleteReservationEndpoint(ILogger logger, IReservationService reservationService)
    {
        _logger = logger;
        _reservationService = reservationService;
    }

    public override void Configure()
    {
        Delete("/reservation/{ReservationId}");
    }

    public override async Task HandleAsync(DeleteReservationRequest req, CancellationToken ct)
    {
        ReservationModel? reservation = await _reservationService.Get(req.ReservationId);

        string? claimUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        if (claimUserId is null)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        UserId userId = UserId.Parse(claimUserId);

        _logger.Information("User {UserId} is attempting to delete reservation {ReservationId}", userId, req.ReservationId);

        if (reservation is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (reservation.ReservationMadeByUser.Id != userId)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        _logger.Information("Delted reservation {ReservationId}", req.ReservationId);

        await _reservationService.Delete(req.ReservationId);
        await SendOkAsync(ct);
    }
}