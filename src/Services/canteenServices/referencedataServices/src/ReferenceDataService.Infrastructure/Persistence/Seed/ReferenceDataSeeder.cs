using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReferenceDataService.Domain.Entities;

namespace ReferenceDataService.Infrastructure.Persistence.Seed;

public static class ReferenceDataSeeder
{
    public static async Task SeedAsync(ReferenceDataDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();

            if (!await context.LovTypeMasters.AnyAsync())
            {
                var lovTypes = new List<LovTypeMaster>
                {
                    new("UNT", "Unit"),
                    new("CAT", "Category"),
                    new("STS", "Status"),
                    new("CUR", "Currency"),
                    new("PAY", "Payment Mode")
                };

                await context.LovTypeMasters.AddRangeAsync(lovTypes);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} LOV Type Masters", lovTypes.Count);
            }

            if (!await context.LovMasters.AnyAsync())
            {
                var lovMasters = new List<LovMaster>
                {
                    new("KG ", "UNT", "Kilogram"),
                    new("LTR", "UNT", "Litre"),
                    new("PCS", "UNT", "Pieces"),
                    new("VEG", "CAT", "Vegetarian"),
                    new("NVG", "CAT", "Non-Vegetarian"),
                    new("ACT", "STS", "Active"),
                    new("INA", "STS", "Inactive"),
                    new("INR", "CUR", "Indian Rupee"),
                    new("USD", "CUR", "US Dollar"),
                    new("CSH", "PAY", "Cash"),
                    new("CRD", "PAY", "Card")
                };

                await context.LovMasters.AddRangeAsync(lovMasters);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} LOV Masters", lovMasters.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}
