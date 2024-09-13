using Mailing;
using MassTransit;

namespace Fahrzeugverwaltung.Startup;

public static class MassTransitRegistration
{
    public static void RegisterMassTransit(this IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(IAssemblyMarker).Assembly);
            x.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); });
        });
    }
}