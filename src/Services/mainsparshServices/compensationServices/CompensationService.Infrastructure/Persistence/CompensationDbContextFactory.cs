using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CompensationService.Infrastructure.Persistence;

/// <summary>
/// Design-time factory for EF Core migrations
/// </summary>
public class CompensationDbContextFactory : IDesignTimeDbContextFactory<CompensationDbContext>
{
    public CompensationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<CompensationDbContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;Integrated Security=True;TrustServerCertificate=True";
        optionsBuilder.UseSqlServer(connectionString);

        return new CompensationDbContext(optionsBuilder.Options);
    }
}
