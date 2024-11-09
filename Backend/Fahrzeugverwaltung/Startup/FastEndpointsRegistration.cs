using System.Text.Json.Serialization;
using Fahrzeugverwaltung.Endpoints.GlobalPreprocessor;

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
                    endpointConfigurator.PreProcessor<ResolveUserFromClaimPreProcessor>(Order.Before);
                    if (!configuration.AuthenticationEnabled)
                    {
                        endpointConfigurator.AllowAnonymous();
                        endpointConfigurator.PreProcessor<AddUserClaimToRequestPreProcessor>(Order.Before);
                        
                    }
                };

                opt.Serializer.Options.Converters.Add(new JsonStringEnumConverter());
                opt.Serializer.Options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });
    }
}