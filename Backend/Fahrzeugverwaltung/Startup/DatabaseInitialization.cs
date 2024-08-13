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
        var usermanager = app.Services.CreateScope().ServiceProvider.GetRequiredService<UserManager<UserModel>>();
        
        database.Database.Migrate();
        database.SaveChanges();

        if (!database.VehicleModels.Any())
        {
            database.VehicleModels.Add(new VehicleModel {Id = VehicleModelId.New(), Name = "Initial Vehicle"});
            database.SaveChanges();
        }

        if (!database.Roles.Any())
        {
            database.Roles.Add(new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = Security.AdminRoleName
            });
            database.SaveChanges();
        }

        if (!database.Users.Any())
        {
            string email = "admin@example.com";
            var result = await usermanager.CreateAsync(new UserModel()
            {
                Firstname = "Admin",
                Lastname = "",
                Organization = string.Empty,
                Email = email,
                UserName = email
            }, "Admin123!");
            var dbUser = await usermanager.FindByEmailAsync(email) ?? throw new ArgumentNullException();
            
            Console.WriteLine(dbUser.Email);
            Console.WriteLine(Security.AdminRoleName);
            
            database.UserRoles.Add(new IdentityUserRole<Guid>
            {
                UserId = dbUser.Id,
                RoleId = database.Roles.First(x => x.Name == Security.AdminRoleName).Id
            });
            await database.SaveChangesAsync();
            
            await usermanager.AddToRolesAsync(dbUser, [Security.AdminRoleName]);
        }
    }
}