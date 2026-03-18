using LeaveServices.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LeaveServices.Infrastructure.Persistence;

public static class DbMigrator
{
    public static async Task MigrateAndSeedAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<LeaveDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<LeaveDbContext>>();

        try
        {
            logger.LogInformation("Applying database migrations...");
            await context.Database.MigrateAsync();
            await SeedDataAsync(context, logger);
            logger.LogInformation("Database migrations applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating the database.");
            throw;
        }
    }

    private static async Task SeedDataAsync(LeaveDbContext context, ILogger logger)
    {
        if (await context.LeaveCounters.AnyAsync()) return;

        logger.LogInformation("Seeding initial data...");

        context.LeaveCounters.AddRange(
            Domain.Entities.LeaveCounter.Create("LET", "Leave Request Counter"),
            Domain.Entities.LeaveCounter.Create("ENC", "Encashment Counter"),
            Domain.Entities.LeaveCounter.Create("LOP", "Loss of Pay Counter")
        );

        await context.SaveChangesAsync();
        logger.LogInformation("Seed data inserted.");
    }
}
