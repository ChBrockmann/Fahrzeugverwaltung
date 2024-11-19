using Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Model;
using Microsoft.EntityFrameworkCore;

namespace Fahrzeugverwaltung.Microservice.Benachrichtigung.Service.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> dbContextOptions) : base(dbContextOptions)
    {
        
    }
    
    public DbSet<UserNotificationSettings> UserNotificationSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserNotificationSettings>()
            .HasKey(x => x.UserId);
    }
}