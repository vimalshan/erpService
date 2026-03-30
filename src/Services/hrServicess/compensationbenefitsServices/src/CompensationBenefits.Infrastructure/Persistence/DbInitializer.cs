using CompensationBenefits.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CompensationBenefits.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitialiseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<CompensationBenefitsDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CompensationBenefitsDbContext>>();

        try
        {
            // Applies pending EF migrations (creates DB + all tables if they don't exist).
            if (context.Database.IsSqlServer())
                await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while migrating or seeding the database");
            throw;
        }

        await SeedAsync(context, logger);
    }

    private static async Task SeedAsync(CompensationBenefitsDbContext context, ILogger logger)
    {
        try
        {
            // Seed sample Salary Structure if none exists
            if (!await context.SalaryStructureMains.AnyAsync())
            {
                // Use raw SQL to bypass EF identity tracking and insert seed data properly
                await context.Database.ExecuteSqlRawAsync("""
                    INSERT INTO [SALSTRUCTURE_MAIN] (STRUCTURE_ID, STRUCTURE_UNITID, STRUCTURE_NAME, STRUCTURE_GRADECATEGORY,
                        STRUCTURE_APPLYTOALL, STRUCTURE_GRADEID, STRUCTURE_TYPE, STRUCTURE_CTCMIN, STRUCTURE_CTCMAX,
                        STRUCTURE_FOOTERID, STRUCTURE_CREATEDBY, STRUCTURE_CREATEDON, STRUCTURE_LASTMODIFIEDBY, STRUCTURE_LASTMODIFIEDON)
                    VALUES (1, 1, N'Standard CTC Structure - Grade A', N'GRA', 0, 1, N'C', 300000, 2000000, 1, 1, GETUTCDATE(), 1, GETUTCDATE());
                    """);
                logger.LogInformation("Seeded default SalaryStructure.");
            }

            // Seed sample Retiral Range Master
            if (!await context.RetiralRangeMasters.AnyAsync())
            {
                await context.Database.ExecuteSqlRawAsync("""
                    INSERT INTO [RETRIALS_RANGEMAST] (RRMAST_ID, RRMAST_UNITID, RRMAST_FROMYEAR, RRMAST_TOYEAR, RRMAST_PERCENTAGE, RRMAST_MODIFIEDBY, RRMAST_MODIFIEDON)
                    VALUES (1, 1, 0, 5, 12, 1, GETUTCDATE());
                    """);
                logger.LogInformation("Seeded default RetiralRangeMaster.");
            }
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Seeding skipped or partially failed — application will continue.");
        }
    }
}
