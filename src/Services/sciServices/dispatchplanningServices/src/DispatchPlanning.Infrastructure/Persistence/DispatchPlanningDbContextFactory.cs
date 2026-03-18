using DispatchPlanning.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DispatchPlanning.Infrastructure;

/// <summary>Used by EF Core tools at design time (migrations).</summary>
public class DispatchPlanningDbContextFactory : IDesignTimeDbContextFactory<DispatchPlanningDbContext>
{
    public DispatchPlanningDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "DispatchPlanning.API"))
            .AddJsonFile("appsettings.json", optional: false)
            .AddEnvironmentVariables()
            .Build();

        var options = new DbContextOptionsBuilder<DispatchPlanningDbContext>()
            .UseSqlServer(
                configuration.GetConnectionString("SCIDB"),
                sql => sql.MigrationsAssembly(typeof(DispatchPlanningDbContext).Assembly.FullName))
            .Options;

        return new DispatchPlanningDbContext(options);
    }
}
