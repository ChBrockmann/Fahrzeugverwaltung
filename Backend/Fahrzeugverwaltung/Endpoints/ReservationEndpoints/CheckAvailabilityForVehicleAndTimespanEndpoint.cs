using DataAccess.ReservationService;
using FluentValidation.Results;
using Microsoft.AspNetCore.Components.Forms;
using Model.Reservation;
using Model.Reservation.Requests;
using Model.Reservation.Responses;

namespace Fahrzeugverwaltung.Endpoints.ReservationEndpoints;

public class CheckAvailabilityForVehicleAndTimespanEndpoint : Endpoint<CheckAvailabilityForVehicleAndTimespanRequest, CheckAvailabilityForVehicleAndTimespanResponse>
{
    private readonly IMapper _mapper;
    private readonly IReservationService _reservationService;
    private readonly IValidator<CreateReservationRequest> _validator;
    private readonly ILogger _logger;

    public CheckAvailabilityForVehicleAndTimespanEndpoint(IReservationService reservationService, IMapper mapper, IValidator<CreateReservationRequest> validator, ILogger logger)
    {
        _reservationService = reservationService;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public override void Configure()
    {
        Get("reservation/checkAvailability/{VehicleId}");
    }

    public override async Task HandleAsync(CheckAvailabilityForVehicleAndTimespanRequest req, CancellationToken ct)
    {
        CreateReservationRequest mappedForValidator = new()
        {
            Vehicle = req.VehicleId,
            EndDateInclusive = req.EndDateInclusive,
            StartDateInclusive = req.StartDateInclusive
        };
        ValidationResult? validationResult = await _validator.ValidateAsync(mappedForValidator, ct);
        
        
        IEnumerable<ReservationModel>? blockingReservations = await _reservationService.GetReservationsInTimespan(req.StartDateInclusive, req.EndDateInclusive, req.VehicleId);

        if (blockingReservations is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        List<ReservationModelDto> blockingReservationsMapped = _mapper.Map<List<ReservationModelDto>>(blockingReservations.ToList());

        CheckAvailabilityForVehicleAndTimespanResponse response = new()
        {
            BlockingReservations = blockingReservationsMapped,
            Errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList(),
            IsAvailable = CheckAvailability(blockingReservationsMapped, validationResult)
        };

        await SendOkAsync(response, ct);
    }

    private bool CheckAvailability(List<ReservationModelDto> blockingReservationsMapped, ValidationResult? validationResult)
    {
        return blockingReservationsMapped.Count == 0 && (validationResult?.IsValid ?? true);
    }
}