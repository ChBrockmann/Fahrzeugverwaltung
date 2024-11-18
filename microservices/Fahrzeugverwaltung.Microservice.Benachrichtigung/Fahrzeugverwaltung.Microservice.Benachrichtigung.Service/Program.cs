using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Hubs;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;
using MassTransit;
using Serilog;
using Serilog.Core;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration
    .AddJsonFile("./configuration/appsettings.json", true, true)
    .AddUserSecrets(typeof(Program).Assembly, true, true)
    .AddEnvironmentVariables();

builder.Services.Configure<Configuration>(builder.Configuration);
Configuration? configuration = builder.Configuration.Get<Configuration>();
if (configuration is null) throw new Exception("Configuration is null");


Logger logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {SourceContext} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("./logs/info/info-.txt", LogEventLevel.Information,
        rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
    .WriteTo.File("./logs/error/error-.txt", LogEventLevel.Error,
        rollingInterval: RollingInterval.Month, retainedFileCountLimit: 12)
    .CreateLogger();
builder.Services.AddSerilog(logger);


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