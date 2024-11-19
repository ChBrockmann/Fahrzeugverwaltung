using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Database;
using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Startup;

public static class StartupDatabase
{
    public static void InitializeDatabase(this IServiceCollection services, Configuration configuration)
    {
        services.AddDbContext<DatabaseContext>(options =>
        {
            string connectionString = configuration.DatabaseConnectionString;
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), builder => builder.EnableRetryOnFailure());
        });
        
        using var scope = services.BuildServiceProvider().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        dbContext.Database.Migrate();
    }
}