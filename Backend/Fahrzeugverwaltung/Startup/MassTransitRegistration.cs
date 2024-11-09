using MassTransit;
using Model.Configuration;

namespace Fahrzeugverwaltung.Startup;

public static class MassTransitRegistration
{
    public static void RegisterMassTransit(this IServiceCollection services, Configuration configuration)
    {
        RabbitMqConfiguration rabbitMqConfiguration = configuration.RabbitMq;
        
        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqConfiguration.Host, rabbitMqConfiguration.Port, rabbitMqConfiguration.VirtualHost, c =>
                {
                    c.Username(rabbitMqConfiguration.Username);
                    c.Password(rabbitMqConfiguration.Password);
                });

                cfg.ConfigureEndpoints(context);
            });
        });
    }
}