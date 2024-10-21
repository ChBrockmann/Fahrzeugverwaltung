using Microsoft.EntityFrameworkCore;
using Model.Invitation;
using Model.LogBook;
using Model.Organization;
using Model.Reservation;
using Model.ReservationStatus;
using Model.Roles;
using Model.User;
using Model.Vehicle;

namespace DataAccess;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<ReservationModel> ReservationModels { get; set; } = null!;

    public DbSet<VehicleModel> VehicleModels { get; set; } = null!;
    public DbSet<UserModel> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<ReservationStatusModel> ReservationStatusModels { get; set; } = null!;
    public DbSet<InvitationModel> InvitationModels { get; set; } = null!;
    public DbSet<LogBookEntry> LogBookEntries { get; set; } = null!;
    public DbSet<OrganizationModel> Organizations { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<InvitationModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new InvitationId(x));
        modelBuilder.Entity<InvitationModel>()
            .HasOne(x => x.CreatedBy);
        modelBuilder.Entity<InvitationModel>()
            .HasMany(x => x.Roles)
            .WithMany();

        modelBuilder.Entity<UserModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new UserId(x));
        modelBuilder.Entity<UserModel>()
            .HasMany(u => u.ReservationsMadeByUser)
            .WithOne(u => u.ReservationMadeByUser);
        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.ReservationStatusChanges)
            .WithOne(x => x.StatusChangedByUser);
        modelBuilder.Entity<UserModel>()
            .HasOne(x => x.Organization)
            .WithMany(x => x.Users);
        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users);

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

        modelBuilder.Entity<OrganizationModel>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new OrganizationId(x));
        modelBuilder.Entity<OrganizationModel>()
            .HasKey(u => u.Id);
        modelBuilder.Entity<OrganizationModel>()
            .HasMany(x => x.Admins)
            .WithMany();
        modelBuilder.Entity<OrganizationModel>()
            .HasMany(x => x.Users)
            .WithOne(x => x.Organization);

        modelBuilder.Entity<Role>()
            .HasKey(x => x.Name);

        modelBuilder.Entity<LogBookEntry>()
            .Property(x => x.Id)
            .HasConversion(x => x.Value, x => new LogBookEntryId(x));
        modelBuilder.Entity<LogBookEntry>()
            .HasOne(x => x.CreatedBy)
            .WithMany();
        modelBuilder.Entity<LogBookEntry>()
            .HasOne(x => x.AssociatedReservation)
            .WithMany();
        modelBuilder.Entity<LogBookEntry>()
            .HasOne(x => x.AssociatedVehicle)
            .WithMany();
    }
}