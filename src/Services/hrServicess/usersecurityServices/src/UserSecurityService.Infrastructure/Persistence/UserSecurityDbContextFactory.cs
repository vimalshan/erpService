using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace UserSecurityService.Infrastructure.Persistence;

/// <summary>Allows dotnet-ef to create migrations without a running host.</summary>
public class UserSecurityDbContextFactory : IDesignTimeDbContextFactory<UserSecurityDbContext>
{
    public UserSecurityDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=HRDB;Integrated Security=True;TrustServerCertificate=True;";

        var optionsBuilder = new DbContextOptionsBuilder<UserSecurityDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new UserSecurityDbContext(optionsBuilder.Options);
    }
}
