global using FastEndpoints;
global using IMapper = MapsterMapper.IMapper;
global using ILogger = Serilog.ILogger;
global using FluentValidation;
using Fahrzeugverwaltung.Startup;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Model.Configuration;
using Model.User;

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
builder.Services
    .AddAuthorization();
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;
    opt.Password.RequireNonAlphanumeric = false;

    opt.User.RequireUniqueEmail = false;
});

builder.Services.ConfigureApplicationCookie(opt =>
{
    opt.ExpireTimeSpan = TimeSpan.FromHours(configuration.CookieExpirationInHours);
    opt.AccessDeniedPath = "/Identity/Account/AccessDenied";
    opt.Cookie.Name = "YourAppCookieName";
    opt.Cookie.HttpOnly = true;
    opt.LoginPath = "/Identity/Account/Login";
    opt.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    opt.SlidingExpiration = true;
    opt.Cookie.IsEssential = true;
});

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

app.MapGroup("api/identity").MapIdentityApi<UserModel>();

app.UseHttpsRedirection();

if (authenticationEnabled)
{
    app.UseAuthentication();
    app.UseAuthorization();
}

app.SetupFastEndpoints(configuration);

if (app.Environment.IsDevelopment()) app.UseSwaggerGen();

app.Run();