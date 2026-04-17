using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmailNotification.Infrastructure.Data;

/// <summary>
/// Factory for creating EmailNotificationDbContext instances at design time
/// This is used by Entity Framework Core tooling for migrations
/// </summary>
public class EmailNotificationDbContextFactory : IDesignTimeDbContextFactory<EmailNotificationDbContext>
{
    /// <summary>
    /// Creates a DbContext instance for use by EF Core tools
    /// </summary>
    public EmailNotificationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<EmailNotificationDbContext>();
        
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .Build();
        var connectionString = config.GetConnectionString("DefaultConnection")
            ?? "Server=.;Database=EmailNotificationService;Trusted_Connection=true;TrustServerCertificate=true;";
        
        optionsBuilder.UseSqlServer(connectionString);
        
        return new EmailNotificationDbContext(optionsBuilder.Options);
    }
}
