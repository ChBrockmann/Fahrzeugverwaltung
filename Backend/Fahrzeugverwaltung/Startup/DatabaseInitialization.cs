using DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model;
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
            var roleManager = app.Services.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            await roleManager.CreateAsync(new IdentityRole<Guid>(Security.AdminRoleName));
        }

        if (!database.Users.Any())
        {
            var usermanager = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<UserModel>>();
            string email = "admin@example.com";
            
            await usermanager.CreateAsync(new UserModel()
            {
                Firstname = "Admin",
                Lastname = "",
                Organization = string.Empty,
                Email = email,
                UserName = email
            }, "Admin123!");
            var dbUser = await usermanager.FindByEmailAsync(email) ?? throw new ArgumentNullException();
            
            await usermanager.AddToRolesAsync(dbUser, [Security.AdminRoleName]);
        }
    }
}