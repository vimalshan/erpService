using Microsoft.EntityFrameworkCore;
using PromotionService.Domain.Entities;
using PromotionService.Infrastructure.Persistence;

namespace PromotionService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(PromotionDbContext context)
    {
        await context.Database.MigrateAsync();

        // DD_PROMOTIONPERIOD is a keyless entity — seed via raw SQL to avoid EF tracking requirement
        var periodCount = await context.PromotionPeriods.CountAsync();
        if (periodCount == 0)
        {
            await context.Database.ExecuteSqlRawAsync(@"
                INSERT INTO DD_PROMOTIONPERIOD (DD_PRM_ID, DD_PRD_DSC)
                VALUES
                    (1, 'Annual 2024-25'),
                    (2, 'Mid-Year 2025'),
                    (3, 'Annual 2025-26')");
        }

        if (!await context.AppraisalAmounts.AnyAsync())
        {
            context.AppraisalAmounts.AddRange(
                new AppraisalAmount
                {
                    SerialNo = 1,
                    BandId = 1,
                    VtcRating = "H1",
                    BandPercentage = 15,
                    BandMinAmount = 50000,
                    BandMaxAmount = 200000,
                    GradeCode = "A",
                    GradeId = 1,
                    AppraisalPeriodFrom = new DateTime(2025, 4, 1),
                    AppraisalPeriodTo = new DateTime(2026, 3, 31),
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow
                },
                new AppraisalAmount
                {
                    SerialNo = 2,
                    BandId = 2,
                    VtcRating = "H2A",
                    BandPercentage = 10,
                    BandMinAmount = 40000,
                    BandMaxAmount = 150000,
                    GradeCode = "B",
                    GradeId = 2,
                    AppraisalPeriodFrom = new DateTime(2025, 4, 1),
                    AppraisalPeriodTo = new DateTime(2026, 3, 31),
                    ModifiedBy = 0,
                    ModifiedOn = DateTime.UtcNow
                }
            );
            await context.SaveChangesAsync();
        }
    }
}
