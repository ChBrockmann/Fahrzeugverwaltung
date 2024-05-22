using System.Text.Json.Serialization;
using Fahrzeugverwaltung.Endpoints.GlobalPreprocessor;
using Model.Configuration;

namespace Fahrzeugverwaltung.Startup;

public static class FastEndpointsRegistration
{
    public static void SetupFastEndpoints(this WebApplication app, Configuration configuration)
    {
        app
            .UseDefaultExceptionHandler()
            .UseFastEndpoints(opt =>
            {
                opt.Endpoints.RoutePrefix = "api";
                opt.Endpoints.ShortNames = true;

                opt.Endpoints.Configurator = endpointConfigurator =>
                {
                    if (!configuration.AuthenticationEnabled)
                    {
                        endpointConfigurator.AllowAnonymous();
                        endpointConfigurator.PreProcessor<AddUserClaimToRequestPreProcessor>(Order.Before);
                    }
                };

                opt.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
            });
    }
}