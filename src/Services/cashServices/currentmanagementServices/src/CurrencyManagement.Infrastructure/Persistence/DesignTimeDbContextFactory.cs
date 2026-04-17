using System.IO;

using Microsoft.Extensions.Configuration;
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
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CASHDB;Integrated Security=True;TrustServerCertificate=True";
        
        optionsBuilder.UseSqlServer(connectionString);
        
        return new CurrencyDbContext(optionsBuilder.Options);
    }
}
