using Contracts.Mailing;
using DataAccess.ReservationService;
using DataAccess.UserService;
using DataAccess.VehicleService;
using Fahrzeugverwaltung.Extensions;
using FluentValidation.Results;
using MassTransit;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class CreateReservationEndpoint : BaseEndpoint<CreateReservationRequest, ReservationModelDto>
{
    private readonly ILogger<CreateReservationEndpoint> _logger;
    private readonly IMapper _mapper;
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;
    private readonly IVehicleService _vehicleService;
    private readonly IValidator<CreateReservationRequest> _validator;
    private readonly IPublishEndpoint _bus;

    public CreateReservationEndpoint(IMapper mapper, IReservationService reservationService, 
        IVehicleService vehicleService, ILogger<CreateReservationEndpoint> logger,
        IUserService userService, IValidator<CreateReservationRequest> validator, IPublishEndpoint bus)
    {
        _mapper = mapper;
        _reservationService = reservationService;
        _vehicleService = vehicleService;
        _logger = logger;
        _userService = userService;
        _validator = validator;
        _bus = bus;
    }

    public override void Configure()
    {
        Post("reservation");
    }

    public override async Task HandleAsync(CreateReservationRequest req, CancellationToken ct)
    {
        var validationResult = await _validator.ValidateAsync(req, ct);
        if (!validationResult.IsValid)
        {
            ValidationFailures.AddRange(validationResult.Errors);
            ThrowIfAnyErrors();
        }
        
        VehicleModel requestedVehicle = await _vehicleService.Get(req.Vehicle) ?? throw new ArgumentNullException(nameof(req.Vehicle), "Vehicle not found");

        IEnumerable<ReservationModel> existingReservation = await _reservationService.GetReservationsInTimespanWithoutDenied(req.StartDateInclusive, req.EndDateInclusive, req.Vehicle) ?? throw new ArgumentNullException();
        List<ReservationModel> existingReservationList = existingReservation.ToList();

        if (existingReservationList.Any())
        {
            _logger.LogWarning("Requested vehicle is alredy reserved. Request-Startdate: {StartDate} Request-Enddate: {EndDate}, existing reservation: {ExistingReservationId}",
                req.StartDateInclusive.ToIso8601(), req.EndDateInclusive.ToIso8601(), string.Join(",", existingReservationList.Select(x => x.Id)).Trim(','));
            ThrowError(new ValidationFailure(nameof(req.Vehicle), "The requested vehicle is already reserved for the requested time"));
        }

        ReservationModel mapped = _mapper.Map<ReservationModel>(req);
        mapped.Id = ReservationId.New();
        mapped.VehicleReserved = requestedVehicle;
        mapped.ReservationMadeByUser = UserFromContext;

        ReservationModel result = await _reservationService.Create(mapped);
        await _bus.Publish(new SendNewReservationMail
        {
            ReservationId = mapped.Id
        }, ct);

        await SendOkAsync(_mapper.Map<ReservationModelDto>(result), ct);
    }
}