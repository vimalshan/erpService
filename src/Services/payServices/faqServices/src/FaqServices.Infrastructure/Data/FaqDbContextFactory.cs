using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FaqServices.Infrastructure.Data;

public class FaqDbContextFactory : IDesignTimeDbContextFactory<FaqDbContext>
{
    public FaqDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FaqDbContext>();
        
        // Default connection string for migrations
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=FaqDb;Trusted_Connection=true;";
        
        optionsBuilder.UseSqlServer(
            connectionString,
            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")
        );

        return new FaqDbContext(optionsBuilder.Options);
    }
}
