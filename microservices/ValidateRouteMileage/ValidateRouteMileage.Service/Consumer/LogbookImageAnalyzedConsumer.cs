using Contracts;
using MassTransit;
using ValidateRouteMileage.BusinessLogic;
using ValidateRouteMileage.Service.ExternalServices.GoogleMaps;

namespace ValidateRouteMileage.Service.Consumer;

public class LogbookImageAnalyzedConsumer : IConsumer<LogbookImageAnalyzed>
{
    private readonly IGoogleMapsServiceWrapper _googleMapsService;
    private readonly AllowedMaximumMileageCalculator _calculator;
    

    public LogbookImageAnalyzedConsumer(IGoogleMapsServiceWrapper googleMapsService, AllowedMaximumMileageCalculator calculator)
    {
        _googleMapsService = googleMapsService;
        _calculator = calculator;
    }

    public async Task Consume(ConsumeContext<LogbookImageAnalyzed> context)
    {
        var googleMapsResult = await _googleMapsService.GetRouteDistance(context.Message.OriginAdress, context.Message.DestinationAdress);

        var maximumAllowedMileage = _calculator.Calculate(context.Message.StartDateInclusive, context.Message.EndDateInclusive, googleMapsResult);

        string result = context.Message.TotalMileageForRoute > maximumAllowedMileage ? "Exceeded" : "Within limits";
        
        Console.WriteLine($"Result: {result}");
    }
}