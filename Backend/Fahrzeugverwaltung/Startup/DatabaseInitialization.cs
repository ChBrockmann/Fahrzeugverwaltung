using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Model;
using Model.Organization;
using Model.Roles;
using Model.User;
using Model.Vehicle;

namespace Fahrzeugverwaltung.Startup;

public static class DatabaseInitialization
{
    public static async Task InitializeDatabase(this WebApplication app)
    {
        DatabaseContext database = app.Services.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
        
        await database.Database.MigrateAsync();
        await database.SaveChangesAsync();

        if (!database.VehicleModels.Any())
        {
            database.VehicleModels.Add(new VehicleModel {Id = VehicleModelId.New(), Name = "Initial Vehicle"});
            await database.SaveChangesAsync();
        }

        if (!database.Roles.Any())
        {
            await database.Roles.AddRangeAsync(new Role {Name = SecurityConfiguration.UserRoleName}, new Role {Name = SecurityConfiguration.OrganizationAdminRoleName}, new Role {Name = SecurityConfiguration.AdminRoleName});
            await database.SaveChangesAsync();
        }

        if (!database.Users.Any())
        {
            string email = "admin@example.com";

            EntityEntry<UserModel> user = await database.Users.AddAsync(new UserModel
            {
                Id = UserId.New(),
                Firstname = "Admin",
                Lastname = string.Empty,
                Organization = new OrganizationModel
                {
                    Id = OrganizationId.New(),
                    Name = "Initial Organization",
                    Description = "This is the default organization"
                },
                Roles = new List<Role>
                {
                    new() {Name = SecurityConfiguration.AdminRoleName}
                },
                Email = email,
            });
            await database.SaveChangesAsync();
        }
    }
}