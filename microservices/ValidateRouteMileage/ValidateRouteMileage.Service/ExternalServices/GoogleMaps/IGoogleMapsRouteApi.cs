using Refit;
using ValidateRouteMileage.Service.ExternalServices.GoogleMaps.RequestResponse;

namespace ValidateRouteMileage.Service.ExternalServices.GoogleMaps;

internal interface IGoogleMapsRouteApi
{
    [Get("maps/api/distancematrix/json")]
    Task<GoogleMapsRouteDistanceResponse> GetRouteDistance(string destinations, string origins, string units, string key);
}