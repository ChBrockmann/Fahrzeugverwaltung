using BusinessLogic.Validators;
using BusinessLogic.Validators.Reservation;
using BusinessLogic.Validators.Vehicle;
using DataAccess;
using DataAccess.InvitationService;
using DataAccess.Provider.DateTimeProvider;
using DataAccess.ReservationService;
using DataAccess.ReservationStatusService;
using DataAccess.UserService;
using DataAccess.VehicleService;
using Fahrzeugverwaltung.Endpoints;
using Fahrzeugverwaltung.Endpoints.tmp;
using Fahrzeugverwaltung.Validators.Reservation;
using FastEndpoints.Swagger;
using FluentValidation;
using MapsterMapper;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Model.Configuration;
using Model.Mapping;
using QuestPDF.Infrastructure;

namespace Fahrzeugverwaltung.Startup;

public static class ServiceRegistration
{
    public static Configuration InitializeConfiguration(this WebApplicationBuilder webApplicationBuilder)
    {
        webApplicationBuilder
            .Configuration
            .AddJsonFile("./configuration/appsettings.json", true, true)
            .AddUserSecrets(typeof(ServiceRegistration).Assembly, true, true)
            .AddEnvironmentVariables();

        webApplicationBuilder.Services.Configure<Configuration>(webApplicationBuilder.Configuration);
        Configuration? configuration = webApplicationBuilder.Configuration.Get<Configuration>();
        if (configuration is null) throw new Exception("Configuration is null");

        return configuration;
    }

    public static void RegisterAllServices(this IServiceCollection services, ILogger logger, Configuration configuration)
    {
        QuestPDF.Settings.License = LicenseType.Community;
        services.AddFastEndpoints();
        services.SwaggerDocument(opt =>
        {
            opt.ShortSchemaNames = true;
            opt.RemoveEmptyRequestSchema = false;
            opt.FlattenSchema = true;
        });
        services.AddValidatorsFromAssemblyContaining<CreateReservationValidator>();

        services.AddSingleton(MappingConfiguration.GetFromAssembliesContaining(typeof(IMappingConfigurationInstaller)));
        services.AddScoped<IMapper, ServiceMapper>();

        services.AddDbContext<DatabaseContext>(options =>
        {
            string connectionString = configuration.DatabaseConnectionString;
            logger.Information("Using connection string: {ConnectionString}", connectionString);
            MySqlServerVersion serverVersion = new(new Version(8, 0, 28));
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder => builder.EnableRetryOnFailure());
            options.EnableSensitiveDataLogging();
        });

        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IReservationStatusService, ReservationStatusService>();
        services.AddScoped<IInvitationService, InvitationService>();

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        services.AddScoped<CreateReservationValidatorLogic>();
        services.AddScoped<VehicleValidator>();


        services.AddMassTransit(x =>
        {
            x.AddConsumers(typeof(TestEndpoint).Assembly);
            // x.AddConsumer<TestConsumer>();
            
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("localhost", "/", c =>
                {
                    c.Username("user");
                    c.Password("password");
                });
                
                cfg.ConfigureEndpoints(context);
            });
        });
        services.AddHostedService<TestWorker>();
    }
}