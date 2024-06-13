using DataAccess.Provider.DateTimeProvider;
using DataAccess.ReservationService;
using Microsoft.Extensions.Options;
using Model.Configuration;
using Model.Vehicle;

namespace BusinessLogic.Validators.Reservation;

public class CreateReservationValidatorLogic
{
    private readonly IReservationService _reservationService;
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    private readonly IDateTimeProvider _dateTimeProvider;
    
    public CreateReservationValidatorLogic(IReservationService reservationService, IOptionsMonitor<Configuration> optionsMonitor, IDateTimeProvider dateTimeProvider)
    {
        _reservationService = reservationService;
        _optionsMonitor = optionsMonitor;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<bool> CheckIfVehicleIsAvailable(VehicleModelId vehicleId, DateOnly startDate, DateOnly endDate, CancellationToken ct)
    {
        var result = await _reservationService.GetReservationsInTimespanWithoutDenied(startDate, endDate, vehicleId);
        if (result is null)
        {
            return true;
        }

        return !result.Any();
    }

    public bool CheckIfStartdateIsAfterToday(DateOnly startDate)
    {
        DateOnly today = _dateTimeProvider.DateToday;
        return startDate >= today;
    }

    public bool CheckReservationAgainstConfiguration(DateOnly startDate, DateOnly endDate)
    {
        return CheckMaxReservationDays(startDate, endDate) &&
               CheckMinReservationDays(startDate, endDate) &&
               CheckMinReservationTimeInAdvance(startDate) &&
               CheckMaxReservationTimeInAdvance(startDate);
    }

    public bool CheckMaxReservationDays(DateOnly startDate, DateOnly endDate)
    {
        int maxReservationDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MaxReservationDays;
        return maxReservationDays == 0 || startDate.AddDays(maxReservationDays) > endDate;
    }

    public bool CheckMinReservationDays(DateOnly startDate, DateOnly endDate)
    {
        int minReservationDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MinReservationDays;
        return minReservationDays == 0 || startDate.AddDays(minReservationDays - 1) <= endDate;
    }

    public bool CheckMinReservationTimeInAdvance(DateOnly startDate)
    {
        DateOnly today = _dateTimeProvider.DateToday;
        int minReservationTimeInAdvanceInDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MinReservationTimeInAdvanceInDays;

        return minReservationTimeInAdvanceInDays == 0 || startDate >= today.AddDays(minReservationTimeInAdvanceInDays);
    }

    public bool CheckMaxReservationTimeInAdvance(DateOnly startDate)
    {
        int maxReservationTimeInAdvanceInDays = _optionsMonitor.CurrentValue.ReservationRestrictions.MaxReservationTimeInAdvanceInDays;
        DateOnly today = _dateTimeProvider.DateToday;

        DateOnly earliestReservationDate = startDate.AddDays(maxReservationTimeInAdvanceInDays * (-1));

        return maxReservationTimeInAdvanceInDays == 0 || today >= earliestReservationDate;
    }
}