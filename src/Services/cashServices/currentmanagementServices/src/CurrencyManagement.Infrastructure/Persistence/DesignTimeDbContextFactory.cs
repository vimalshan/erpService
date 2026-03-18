using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CurrencyManagement.Infrastructure.Persistence;

/// <summary>
/// Design-time DbContext factory for EF Core migrations
/// This allows migrations to be created without full DI container initialization
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CurrencyDbContext>
{
    public CurrencyDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CurrencyDbContext>();
        
        // Use LocalDB for design-time
        var connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CASHDB;Integrated Security=True;" +
                              "Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;" +
                              "Encrypt=True;TrustServerCertificate=False;Application Name=\"CurrencyManagement.API\";" +
                              "Command Timeout=0";
        
        optionsBuilder.UseSqlServer(connectionString);
        
        return new CurrencyDbContext(optionsBuilder.Options);
    }
}
