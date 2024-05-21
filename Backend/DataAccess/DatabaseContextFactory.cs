using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<DatabaseContext> optionsbuilder = new();

        string connectionString = "server=localhost;database=fahrzeugverwaltung;user=root;password=root";
        MySqlServerVersion serverVersion = new(new Version(8, 0, 28));
        optionsbuilder.UseMySql(connectionString, serverVersion);

        return new DatabaseContext(optionsbuilder.Options);
    }
}