using Microsoft.Extensions.Options;
using ValidateRouteMileage.Model.Configuration;
using ValidateRouteMileage.Model.Route;

namespace ValidateRouteMileage.BusinessLogic;

public class AllowedMaximumMileageCalculator
{
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    
    
    public AllowedMaximumMileageCalculator(IOptionsMonitor<Configuration> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }
    

    public int Calculate(DateOnly startDateInclusive, DateOnly endDateInclusive, RouteDistanceResult calculatedRouteDistance)
    {
        int result = calculatedRouteDistance.TotalDistanceInMeter;
        var config = _optionsMonitor.CurrentValue.RouteValidationParameters;

        result += GetAdditionalDistance(startDateInclusive.ToDateTime(new TimeOnly(0)), endDateInclusive.ToDateTime(new TimeOnly(0)));
        result += GetAllowedDeviation(result, config.AllowedDeviationPercent);
        
        return result;
    }
    
    private int GetAllowedDeviation(int totalDistanceInMeter, decimal allowedDeviationPercent)
    {
        return (int) Math.Round(totalDistanceInMeter * (allowedDeviationPercent / 100), MidpointRounding.ToEven);
    }

    private int GetAdditionalDistance(DateTime startDateInclusive, DateTime endDateInclusive)
    {
        var config = _optionsMonitor.CurrentValue.RouteValidationParameters;

        int daysBetween = (endDateInclusive - startDateInclusive).Days + 1;
        
        if (daysBetween >= config.GrantAdditionalDistanceAfterDays)
        {
            return daysBetween * config.AdditionalDistancePerDayInKm;
        }

        return 0;
    }
}