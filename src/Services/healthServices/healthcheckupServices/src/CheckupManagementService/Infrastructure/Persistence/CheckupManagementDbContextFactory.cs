namespace CheckupManagementService.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

/// <summary>
/// Design-time factory for CheckupManagementDbContext
/// Used by EF Core migrations tooling to create DbContext instances
/// </summary>
public class CheckupManagementDbContextFactory : IDesignTimeDbContextFactory<CheckupManagementDbContext>
{
    public CheckupManagementDbContext CreateDbContext(string[] args)
    {
        // Build configuration pointing to appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<CheckupManagementDbContext>();

        var connectionString = configuration.GetConnectionString("HealthDb")
            ?? throw new InvalidOperationException("Connection string 'HealthDb' not found in configuration");

        optionsBuilder.UseSqlServer(connectionString,
            sqlOptions => sqlOptions.EnableRetryOnFailure(maxRetryCount: 3));

        return new CheckupManagementDbContext(optionsBuilder.Options);
    }
}
