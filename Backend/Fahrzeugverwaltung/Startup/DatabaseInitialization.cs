using DataAccess;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Startup;

public static class DatabaseInitialization
{
    public static void InitializeDatabase(this WebApplication app)
    {
        DatabaseContext database = app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        database.Database.Migrate();
        database.SaveChanges();

        if (!database.VehicleModels.Any())
        {
            database.VehicleModels.Add(new VehicleModel() {Id = VehicleModelId.New(), Name = "Initial Vehicle"});
            database.SaveChanges();
        }

        if (!database.Roles.Any())
        {
            database.Roles.Add(new()
            {
                Id = Guid.NewGuid(),
                Name = Security.AdminRoleName,
            });
            database.SaveChanges();
        }
    }
}