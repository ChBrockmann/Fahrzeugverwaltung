global using ILogger = Serilog.ILogger;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Hubs;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Startup;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

Configuration configuration = builder.SetupConfiguration();

ILogger logger = builder.SetupLogger();


builder.Services.AddSignalR();
builder.Services.AddMassTransit(config =>
{
    config.AddConsumers(typeof(Program).Assembly);

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(configuration.RabbitMq.Host, configuration.RabbitMq.Port, configuration.RabbitMq.VirtualHost, c =>
        {
            c.Username(configuration.RabbitMq.Username);
            c.Password(configuration.RabbitMq.Password);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapHub<UserHub>("/hubs/userHub");

app.Run();