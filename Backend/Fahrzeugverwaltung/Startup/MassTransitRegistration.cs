using MassTransit;

namespace Fahrzeugverwaltung.Startup;

public static class MassTransitRegistration
{
    public static void RegisterMassTransit(this WebApplicationBuilder builder)
    {
        builder.Services.AddMassTransit(x => { x.UsingInMemory((context, cfg) => { cfg.ConfigureEndpoints(context); }); });
    }
}