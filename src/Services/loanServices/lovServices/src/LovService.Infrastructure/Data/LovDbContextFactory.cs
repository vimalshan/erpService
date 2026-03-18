using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LovService.Infrastructure.Data;

/// <summary>Design-time factory used by EF migrations tooling.</summary>
public class LovDbContextFactory : IDesignTimeDbContextFactory<LovDbContext>
{
    public LovDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("LovDb")
            ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=LOANDB;Integrated Security=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<LovDbContext>();
        optionsBuilder.UseSqlServer(connectionString);
        return new LovDbContext(optionsBuilder.Options);
    }
}
