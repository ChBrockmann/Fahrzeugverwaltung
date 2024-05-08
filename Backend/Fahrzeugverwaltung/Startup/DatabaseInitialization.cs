using DataAccess;
using Microsoft.EntityFrameworkCore;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Startup;

public static class DatabaseInitialization
{
    public static void MigrateDatabase(this WebApplication app)
    {
        DatabaseContext database = app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        database.Database.Migrate();
        database.SaveChanges();

        if (!database.VehicleModels.Any())
        {
            database.VehicleModels.Add(new VehicleModel() {Id = VehicleModelId.New(), Name = "Initial Vehicle"});
            database.SaveChanges();
        }
    }
}