using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Reservation;
using Model.User;
using Model.Vehicle;

namespace DataAccess;

public class DatabaseContext : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<ReservationModel> ReservationModels { get; set; } = null!;

    public DbSet<VehicleModel> VehicleModels { get; set; } = null!;
    public DbSet<UserModel> UserModels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<UserModel>()
            .HasMany(u => u.ReservationsMadeByUser)
            .WithOne(u => u.ReservationMadeByUser);
        
        modelBuilder.Entity<ReservationModel>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<ReservationModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ReservationId(x));
        
        modelBuilder.Entity<VehicleModel>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<VehicleModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new VehicleModelId(x));
        modelBuilder.Entity<VehicleModel>()
            .HasMany(x => x.Reservations)
            .WithOne(x => x.VehicleReserved);
    }
}