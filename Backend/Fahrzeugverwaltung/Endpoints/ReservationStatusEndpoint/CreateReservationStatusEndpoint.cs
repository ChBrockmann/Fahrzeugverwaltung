using Contracts.Mailing;
using DataAccess.ReservationService;
using DataAccess.ReservationStatusService;
using DataAccess.UserService;
using MassTransit;
using Model;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.ReservationStatus;

namespace Fahrzeugverwaltung.Endpoints.ReservationStatusEndpoint;

public class CreateReservationStatusEndpoint : BaseEndpoint<AddStatusToReservationRequest, EmptyResponse>
{
    private readonly ILogger _logger;
    private readonly IReservationService _reservationService;
    private readonly IReservationStatusService _reservationStatusService;
    private readonly IUserService _userService;
    private readonly IPublishEndpoint _bus;

    public CreateReservationStatusEndpoint(IReservationStatusService reservationStatusService, ILogger logger, IReservationService reservationService, IUserService userService, IPublishEndpoint bus)
    {
        _reservationStatusService = reservationStatusService;
        _logger = logger;
        _reservationService = reservationService;
        _userService = userService;
        _bus = bus;
    }

    public override void Configure()
    {
        Post("reservation/{ReservationId}/status");
        Roles(SecurityConfiguration.AdminRoleName, SecurityConfiguration.OrganizationAdminRoleName);
    }

    public override async Task HandleAsync(AddStatusToReservationRequest req, CancellationToken ct)
    {
        ReservationModel? reservation = await _reservationService.Get(req.ReservationId);
        if (reservation is null)
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
            StatusChangedByUser = UserFromContext
        };
        await _reservationStatusService.AddStatusToReservationAsync(status, ct);

        await _bus.Publish(new SendReservationStatusChangedMail
        {
            ReservationId = reservation.Id
        }, ct);

        await SendOkAsync(ct);
    }
}