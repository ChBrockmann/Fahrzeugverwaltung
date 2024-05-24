using System.Security.Claims;
using DataAccess;
using DataAccess.ReservationService;
using DataAccess.UserService;
using DataAccess.VehicleService;
using Fahrzeugverwaltung.Extensions;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.User;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class CreateReservationEndpoint : Endpoint<CreateReservationRequest, ReservationModelDto>
{
    private readonly DatabaseContext _database;
    private readonly ILogger<CreateReservationEndpoint> _logger;
    private readonly IMapper _mapper;
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;
    private readonly IVehicleService _vehicleService;

    public CreateReservationEndpoint(IMapper mapper, IReservationService reservationService, IVehicleService vehicleService, ILogger<CreateReservationEndpoint> logger, IUserService userService, DatabaseContext database)
    {
        _mapper = mapper;
        _reservationService = reservationService;
        _vehicleService = vehicleService;
        _logger = logger;
        _userService = userService;
        _database = database;
    }

    public override void Configure()
    {
        Post("reservation");
    }

    public override async Task HandleAsync(CreateReservationRequest req, CancellationToken ct)
    {
        VehicleModel? requestedVehicle = await _database.VehicleModels.FirstOrDefaultAsync(x => x.Id == req.Vehicle);
        if (requestedVehicle is null)
        {
            _logger.LogWarning("Could not find Vehicle {VehicleId}", req.Vehicle);
            ThrowError(x => x.Vehicle, "Vehicle not found");
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
            _logger.LogWarning("Could not find User {UserId}", userId);
            ThrowError("User not found");
        }

        IEnumerable<ReservationModel>? existingReservation = await _reservationService.GetReservationsInTimespan(req.StartDateInclusive, req.EndDateInclusive, req.Vehicle);
        if (existingReservation is not null && existingReservation.Any())
        {
            _logger.LogWarning("Requested vehicle is alredy reserved. Request-Startdate: {StartDate} Request-Enddate: {EndDate}, existing reservation: {ExistingReservationId}",
                req.StartDateInclusive.ToIso8601(), req.EndDateInclusive.ToIso8601(), string.Join(",", existingReservation.Select(x => x.Id)).Trim(','));
            ValidationFailures.Add(new ValidationFailure("vehicleAlreadyReserved", "The requested vehicle is already reserved for the requested time"));
            await SendErrorsAsync(400, ct);
            return;
        }

        ReservationModel mapped = _mapper.Map<ReservationModel>(req);
        mapped.Id = ReservationId.New();
        mapped.VehicleReserved = requestedVehicle;
        mapped.ReservationMadeByUser = requestingUser;

        ReservationModel result = await _reservationService.Create(mapped);

        await SendOkAsync(_mapper.Map<ReservationModelDto>(result), ct);
    }
}