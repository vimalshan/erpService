using GSTComplianceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace GSTComplianceService.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GstDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<GstDbContext>>();

        try
        {
            if ((await context.Database.GetPendingMigrationsAsync()).Any())
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migrations applied.");
            }

            // Only seed if no data exists
            if (!await context.GstSuppliers.AnyAsync())
            {
                try
                {
                    await SeedSuppliersAsync(context);
                    logger.LogInformation("Seed suppliers created successfully.");
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Seeding suppliers failed - they may already exist or have constraint issues. Continuing...");
                }
            }
            else
            {
                logger.LogInformation("Suppliers already exist in database - skipping seed.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database seeding.");
            // Don't rethrow - let the app continue even if seeding fails
        }
    }

    private static async Task SeedSuppliersAsync(GstDbContext context)
    {
        var suppliers = new[]
        {
            Domain.Entities.GstSupplier.Create(1001, "Tata Consultancy Services Ltd", "tcs@example.com", "Mumbai-OU", null),
            Domain.Entities.GstSupplier.Create(1002, "Infosys Limited", "infosys@example.com", "Bangalore-OU", null),
            Domain.Entities.GstSupplier.Create(1003, "Wipro Technologies", "wipro@example.com", "Hyderabad-OU", null),
        };
        await context.GstSuppliers.AddRangeAsync(suppliers);
        await context.SaveChangesAsync();
    }
}
