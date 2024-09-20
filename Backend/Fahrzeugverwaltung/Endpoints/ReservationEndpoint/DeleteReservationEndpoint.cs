using DataAccess.ReservationService;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.User;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoint;

public class DeleteReservationEndpoint : BaseEndpoint<DeleteReservationRequest, EmptyResponse>
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
        UserId userId = UserFromContext.Id;

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