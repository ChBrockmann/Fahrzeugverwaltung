using Microsoft.Extensions.Options;
using Refit;
using ValidateRouteMileage.Model.Configuration;
using ValidateRouteMileage.Model.Route;

namespace ValidateRouteMileage.Service.ExternalServices.GoogleMaps;

public class GoogleMapsServiceWrapper : IGoogleMapsServiceWrapper
{
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    
    public GoogleMapsServiceWrapper(IOptionsMonitor<Configuration> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public async Task<RouteDistanceResult> GetRouteDistance(string origin, string destination)
    {
        var googleMapsApi = RestService.For<IGoogleMapsRouteApi>("https://maps.googleapis.com");
        var googleMapsConfig = _optionsMonitor.CurrentValue.GoogleMaps;

        var googleMapsResult = await googleMapsApi.GetRouteDistance(destination, origin, "metric", googleMapsConfig.ApiKey);

        return new RouteDistanceResult()
        {
            DestinationAdress = googleMapsResult.DestinationAddresses.First(),
            OriginAdress = googleMapsResult.OriginAddresses.First(),
            TotalDistanceInMeter = googleMapsResult.Rows.First().Elements.First().Distance.Value,
        };
    }
}