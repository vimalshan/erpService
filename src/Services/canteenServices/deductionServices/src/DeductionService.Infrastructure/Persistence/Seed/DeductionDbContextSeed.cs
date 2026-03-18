using DeductionService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DeductionService.Infrastructure.Persistence.Seed;

public static class DeductionDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<DeductionDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DeductionDbContext>>();

        try
        {
            if (context.Database.IsSqlServer())
            {
                await context.Database.MigrateAsync();
            }

            // Seed only if no data exists
            if (!await context.DeductionAccesses.AnyAsync())
            {
                logger.LogInformation("[Seed] Seeding DEDUCTION_ACCESS data...");
                // Initial access entries would be seeded here per business requirements
            }

            logger.LogInformation("[Seed] Database seeding complete.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[Seed] Database seeding failed.");
            throw;
        }
    }
}
