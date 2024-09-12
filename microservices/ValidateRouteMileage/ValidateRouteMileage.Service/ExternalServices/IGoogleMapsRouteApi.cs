using Refit;

namespace ValidateRouteMileage.Service.ExternalServices;

internal interface IGoogleMapsRouteApi
{
    [Get("maps/api/distancematrix/json")]
    Task<RouteDistanceResult> GetRouteDistance(string destinations, string origins, string units, string key);
}