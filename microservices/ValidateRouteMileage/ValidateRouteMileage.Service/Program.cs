using MassTransit;
using ValidateRouteMileage.Model.Configuration;
using ValidateRouteMileage.Service.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder
    .Configuration
    .AddJsonFile("./configuration/appsettings.json", true, true)
    .AddUserSecrets(typeof(Program).Assembly, true, true)
    .AddEnvironmentVariables();

builder.Services.Configure<Configuration>(builder.Configuration);

Configuration? configuration = builder.Configuration.Get<Configuration>();
if (configuration is null) throw new Exception("Configuration is null");

var rabbitMqConfiguration = configuration.RabbitMq;

builder.Services.AddMassTransit(config =>
{
    config.AddConsumers(typeof(Program).Assembly);

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(rabbitMqConfiguration.Host, rabbitMqConfiguration.Port, rabbitMqConfiguration.VirtualHost, c =>
        {
            c.Username(rabbitMqConfiguration.Username);
            c.Password(rabbitMqConfiguration.Password);
        });
        
        cfg.ReceiveEndpoint("Contracts:LogbookImageAnalyzed", e =>
        {
            e.ConfigureConsumer<LogbookImageAnalyzedConsumer>(context);
            e.UseRawJsonDeserializer();
            e.UseRawJsonSerializer();
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.Run();