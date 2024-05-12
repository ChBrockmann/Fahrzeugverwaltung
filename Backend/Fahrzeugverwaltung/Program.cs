global using FastEndpoints;
global using IMapper = MapsterMapper.IMapper;
global using ILogger = Serilog.ILogger;
using DataAccess;
using Fahrzeugverwaltung.Startup;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Identity;
using Model.Configuration;
using Model.User;

var builder = WebApplication.CreateBuilder(args);

var logger = builder.SetupLogger();

Configuration configuration = builder.InitializeConfiguration();
logger.Fatal(configuration.DatabaseConnectionString);

builder.Services.RegisterAllServices(logger, configuration);
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddIdentityApiEndpoints<UserModel>()
    .AddEntityFrameworkStores<DatabaseContext>();
builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;

    opt.User.RequireUniqueEmail = false;
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

var app = builder.Build();

app.InitializeDatabase();
app.UseCors("CorsPolicy");

app.MapGroup("identity").MapIdentityApi<UserModel>();

app.UseHttpsRedirection();

// app.UseAuthentication();

app.SetupFastEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerGen();
}

app.Run();