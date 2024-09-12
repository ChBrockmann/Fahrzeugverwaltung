using Microsoft.Extensions.Options;
using Refit;
using ValidateRouteMileage.Model;
using ValidateRouteMileage.Model.Configuration;

namespace ValidateRouteMileage.Service.ExternalServices;

public class IGoogleMapsServiceWrapper
{
    private readonly IOptionsMonitor<Configuration> _optionsMonitor;
    
    public IGoogleMapsServiceWrapper(IOptionsMonitor<Configuration> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    public async Task<RouteDistanceResult> GetRouteDistance(string origin, string destination)
    {
        var googleMapsApi = RestService.For<IGoogleMapsRouteApi>("https://maps.googleapis.com");
        var googleMapsConfig = _optionsMonitor.CurrentValue.GoogleMaps;

        return await googleMapsApi.GetRouteDistance(destination, origin, "metric", googleMapsConfig.ApiKey);
    }
}