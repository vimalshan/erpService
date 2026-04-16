using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FindingsAPI.Gateway.Infrastructure.Data;

public class FindingsDomainDbContextFactory : IDesignTimeDbContextFactory<FindingsDomainDbContext>
{
    public FindingsDomainDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<FindingsDomainDbContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        return new FindingsDomainDbContext(optionsBuilder.Options);
    }
}
