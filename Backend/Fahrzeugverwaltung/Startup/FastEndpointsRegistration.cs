using System.Text.Json.Serialization;

namespace Fahrzeugverwaltung.Startup;

public static class FastEndpointsRegistration
{
    public static void SetupFastEndpoints(this WebApplication app)
    {
        app.UseFastEndpoints(opt =>
        {
            opt.Endpoints.RoutePrefix = "api";
            opt.Endpoints.ShortNames = true;

            opt.Endpoints.Configurator = endpointConfigurator =>
            {
                // endpointConfigurator.AllowAnonymous();
            };

            opt.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
        });
    }
}