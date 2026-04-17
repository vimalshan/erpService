using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TimesheetService.Infrastructure.Data;

/// <summary>
/// Design-time factory used by EF Core tools (dotnet ef) when no running host is available.
/// Points to the same LocalDB instance used in development.
/// </summary>
public sealed class TimesheetDbContextFactory : IDesignTimeDbContextFactory<TimesheetDbContext>
{
    public TimesheetDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TimesheetDbContext>();
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=SRFSPARSHDB;Integrated Security=True;TrustServerCertificate=True;";
        optionsBuilder.UseSqlServer(connectionString,
            sql => sql.MigrationsAssembly(typeof(TimesheetDbContext).Assembly.FullName)
                      .EnableRetryOnFailure(3, TimeSpan.FromSeconds(5), null));

        return new TimesheetDbContext(optionsBuilder.Options);
    }
}
