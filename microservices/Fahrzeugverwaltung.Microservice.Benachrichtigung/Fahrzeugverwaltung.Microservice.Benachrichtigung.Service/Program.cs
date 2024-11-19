global using ILogger = Serilog.ILogger;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Hubs;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model.Configuration;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Startup;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

Configuration configuration = builder.SetupConfiguration();

ILogger logger = builder.SetupLogger();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{configuration.Keycloak.BaseAuthServerUrl}realms/{configuration.Keycloak.Realm}";
        options.Audience = configuration.Keycloak.Audience;
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true
        };
        
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // Überprüfen, ob die Anfrage zu SignalR gehört
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/hubs"))) 
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    });

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

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<UserHub>("/hubs/userHub");


app.Run();