using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AccessService.Infrastructure.Persistence
{
    /// <summary>
    /// Design-time factory for creating DbContext instances during EF migrations
    /// Allows dotnet ef migrations to work without needing full dependency injection
    /// </summary>
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AccessServiceDbContext>
    {
        public AccessServiceDbContext CreateDbContext(string[] args)
        {
            // Default connection string for development/migration
            var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Integrated Security=True;Initial Catalog=ACCESSDB;";

            var optionsBuilder = new DbContextOptionsBuilder<AccessServiceDbContext>();
            optionsBuilder.UseSqlServer(connectionString, options =>
            {
                options.EnableRetryOnFailure(maxRetryCount: 3);
            });

            return new AccessServiceDbContext(optionsBuilder.Options);
        }
    }
}
