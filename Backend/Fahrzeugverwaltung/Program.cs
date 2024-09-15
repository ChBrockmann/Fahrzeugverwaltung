global using FastEndpoints;
global using IMapper = MapsterMapper.IMapper;
global using ILogger = Serilog.ILogger;
global using FluentValidation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Fahrzeugverwaltung.Startup;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model.Configuration;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ILogger logger = builder.SetupLogger();

Configuration configuration = builder.InitializeConfiguration();
bool authenticationEnabled = configuration.AuthenticationEnabled;

if (authenticationEnabled)
{
    logger.Information("Authentication enabled");
}
else
{
    logger.Warning("Authentication disabled");
}

builder.Services.RegisterMassTransit();
builder.Services.RegisterAllServices(logger, configuration);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://localhost:8080/realms/fahrzeugverwaltung";
        options.Audience = "fahrzeugverwaltung-backend";
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidateLifetime = true,
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        policyBuilder
            .WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

await app.InitializeDatabase();
app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

if (authenticationEnabled)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.SetupFastEndpoints(configuration);

if (app.Environment.IsDevelopment()) app.UseSwaggerGen();

app.Run();