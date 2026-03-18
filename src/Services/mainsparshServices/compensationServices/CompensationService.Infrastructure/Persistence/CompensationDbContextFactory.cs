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
        optionsBuilder.UseSqlServer(
            "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=CompensationServiceDb;Integrated Security=True;TrustServerCertificate=True");

        return new CompensationDbContext(optionsBuilder.Options);
    }
}
