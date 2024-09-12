using ValidateRouteMileage.Model.Route;

namespace ValidateRouteMileage.Service.ExternalServices.GoogleMaps;

public interface IGoogleMapsServiceWrapper
{
    public Task<RouteDistanceResult> GetRouteDistance(string origin, string destination);
}