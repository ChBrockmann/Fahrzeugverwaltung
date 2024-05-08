using DataAccess;
using DataAccess.ReservationService;
using DataAccess.UserService;
using DataAccess.VehicleService;
using FastEndpoints.Swagger;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Model.Mapping;

namespace Fahrzeugverwaltung.Startup;

public static class ServiceRegistration
{
    public static void RegisterAllServices(this IServiceCollection services)
    {
        services.AddFastEndpoints();
        services.SwaggerDocument(opt =>
        {
            opt.ShortSchemaNames = true;
            opt.RemoveEmptyRequestSchema = false;
            opt.FlattenSchema = true;
        });
        
        services.AddSingleton(MappingConfiguration.GetFromAssembliesContaining(typeof(IMappingConfigurationInstaller)));
        services.AddScoped<IMapper, ServiceMapper>();
        
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlite("Data Source=database.sqlite");
        });

        services.AddScoped<IVehicleService, VehicleService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IUserService, UserService>();
    }
}