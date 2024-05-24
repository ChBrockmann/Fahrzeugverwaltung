using DataAccess.ReservationService;
using DataAccess.VehicleService;
using Fahrzeugverwaltung.Provider.DateTimeProvider;
using FluentValidation;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Reservation.Requests;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Validators.Reservation;

public class CreateReservationValidator : Validator<CreateReservationRequest>
{
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    private readonly IReservationService _reservationService;
    private readonly IVehicleService _vehicleService;

    public CreateReservationValidator(IVehicleService vehicleService, IReservationService reservationService, IOptionsMonitor<Configuration> optionsMonitor, IDateTimeProvider dateTimeProvider)
    {
        _vehicleService = vehicleService;
        _reservationService = reservationService;
        _optionsMonitor = optionsMonitor;
        _dateTimeProvider = dateTimeProvider;

        RuleFor(x => x.StartDateInclusive)
            .LessThanOrEqualTo(x => x.EndDateInclusive)
            .WithMessage("Startdate has to be before Enddate")
            .Must((request, startDate) => CheckMaxReservationDays(request.StartDateInclusive, request.EndDateInclusive))
            .WithMessage("Reservation exceeds maximum reservation days")
            .Must((request, startDate) => CheckMinReservationDays(request.StartDateInclusive, request.EndDateInclusive))
            .WithMessage("Reservation is below minimum reservation days")
            .Must(CheckMinReservationTimeInAdvance)
            .WithMessage("Reservation is below minimum reservation time in advance")
            .Must(CheckMaxReservationTimeInAdvance)
            .WithMessage("Reservation is above maximum reservation time in advance");

        RuleFor(x => x.Vehicle)
            .MustAsync(CheckIfVehicleExists)
            .WithMessage("Vehicle does not exist")
            .MustAsync(async (request, vehicleId, ct) =>
                await CheckIfVehicleIsAvailable(vehicleId, request.StartDateInclusive, request.EndDateInclusive, ct))
            .WithMessage("Vehicle is not available in the given time frame");
    }

    private async Task<bool> CheckIfVehicleExists(VehicleModelId vehicleId, CancellationToken ct)
    {
        return await _vehicleService.Exists(vehicleId);
    }

    private async Task<bool> CheckIfVehicleIsAvailable(VehicleModelId vehicleId, DateOnly startDate, DateOnly endDate, CancellationToken ct)
    {
        var result = await _reservationService.GetReservationsInTimespan(startDate, endDate, vehicleId);
        if (result is null)
        {
            return true;
        }

        return !result.Any();
    }

    private bool CheckMaxReservationDays(DateOnly startDate, DateOnly endDate)
    {
        int maxReservationDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MaxReservationDays;
        return maxReservationDays == 0 || startDate.AddDays(maxReservationDays) > endDate;
    }

    private bool CheckMinReservationDays(DateOnly startDate, DateOnly endDate)
    {
        int minReservationDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MinReservationDays;
        return minReservationDays == 0 || startDate.AddDays(minReservationDays - 1) <= endDate;
    }

    private bool CheckMinReservationTimeInAdvance(DateOnly startDate)
    {
        DateOnly today = _dateTimeProvider.DateToday;
        int minReservationTimeInAdvanceInDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MinReservationTimeInAdvanceInDays;

        return minReservationTimeInAdvanceInDays == 0 || startDate >= today.AddDays(minReservationTimeInAdvanceInDays);
    }

    private bool CheckMaxReservationTimeInAdvance(DateOnly startDate)
    {
        int maxReservationTimeInAdvanceInDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MaxReservationTimeInAdvanceInDays;
        DateOnly today = _dateTimeProvider.DateToday;

        DateOnly earliestReservationDate = startDate.AddDays(maxReservationTimeInAdvanceInDays * (-1));

        return maxReservationTimeInAdvanceInDays == 0 || today >= earliestReservationDate;
    }
}