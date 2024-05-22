global using FastEndpoints;
global using IMapper = MapsterMapper.IMapper;
global using ILogger = Serilog.ILogger;
using DataAccess;
using Fahrzeugverwaltung.Startup;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.BearerToken;
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

builder.Services.RegisterAllServices(logger, configuration);
builder.Services.AddAuthorization();
builder.Services
    .AddIdentityApiEndpoints<UserModel>()
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<DatabaseContext>();
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;

    opt.User.RequireUniqueEmail = false;
});
builder.Services.AddOptions<BearerTokenOptions>().Configure(opt => { opt.BearerTokenExpiration = TimeSpan.FromMinutes(configuration.BearerTokenExpirationInMinutes); });


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

app.InitializeDatabase();
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