using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrganizationStructureService.Infrastructure.Persistence;

namespace OrganizationStructureService.Infrastructure.Persistence;

public static class DatabaseInitializer
{
    public static async Task InitializeDatabaseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OrganizationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrganizationDbContext>>();

        try
        {
            logger.LogInformation("Applying database migrations...");
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrations applied successfully.");
            await SeedDataAsync(context, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }

    private static async Task SeedDataAsync(OrganizationDbContext context, ILogger logger)
    {
        if (!await context.HrRoles.AnyAsync())
        {
            logger.LogInformation("Seeding HR Roles...");
            // Seed data is inserted via SQL migrations: see SeedData.sql
        }
    }
}
