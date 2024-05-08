using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess;

public class DatabaseContextFactory : IDesignTimeDbContextFactory<DatabaseContext>
{
    public DatabaseContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<DatabaseContext> optionsbuilder = new();
        
        optionsbuilder.UseSqlite("Data Source=database.sqlite");
        
        return new DatabaseContext(optionsbuilder.Options);
    }
}