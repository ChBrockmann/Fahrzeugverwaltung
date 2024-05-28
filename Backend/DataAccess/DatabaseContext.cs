using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Model.Invitation;
using Model.Reservation;
using Model.ReservationStatus;
using Model.User;
using Model.Vehicle;

namespace DataAccess;

public class DatabaseContext : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<ReservationModel> ReservationModels { get; set; } = null!;

    public DbSet<VehicleModel> VehicleModels { get; set; } = null!;
    public DbSet<UserModel> UserModels { get; set; } = null!;
    public DbSet<ReservationStatusModel> ReservationStatusModels { get; set; } = null!;
    public DbSet<InvitationModel> InvitationModels { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InvitationModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new InivitationId(x));
        modelBuilder.Entity<InvitationModel>()
            .HasOne(x => x.CreatedBy);
        modelBuilder.Entity<InvitationModel>()
            .HasMany(x => x.Roles)
            .WithMany();


        modelBuilder.Entity<UserModel>()
            .HasMany(u => u.ReservationsMadeByUser)
            .WithOne(u => u.ReservationMadeByUser);
        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.ReservationStatusChanges)
            .WithOne(x => x.StatusChangedByUser);

        modelBuilder.Entity<ReservationStatusModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ReservationStatusId(x));

        modelBuilder.Entity<ReservationModel>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<ReservationModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new ReservationId(x));
        modelBuilder.Entity<ReservationModel>()
            .HasMany(x => x.ReservationStatusChanges)
            .WithOne(x => x.Reservation);

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