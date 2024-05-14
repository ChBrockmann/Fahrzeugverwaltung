using DataAccess.ReservationService;
using Model.Reservation.Requests;

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
        Delete("/reservation/{ReservationId}/{UserId}");
    }

    public override async Task HandleAsync(DeleteReservationRequest req, CancellationToken ct)
    {
        var reservation = await _reservationService.Get(req.ReservationId);

        if (reservation is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }
        
        if (reservation.ReservationMadeByUser.Id != req.UserId)
        {
            await SendUnauthorizedAsync(ct);
            return;
        }
        
        await _reservationService.Delete(req.ReservationId);
        await SendOkAsync(ct);
    }
}