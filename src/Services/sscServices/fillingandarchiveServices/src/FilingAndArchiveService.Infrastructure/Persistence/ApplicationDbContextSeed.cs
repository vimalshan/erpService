using FilingAndArchiveService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FilingAndArchiveService.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Seed initial filing counters for business units
        if (!await context.FilingCounters.AnyAsync())
        {
            var counters = new[]
            {
                new FilingCounter { FilingBuId = "HQ001", FileCount = 0 },
                new FilingCounter { FilingBuId = "BR001", FileCount = 0 },
                new FilingCounter { FilingBuId = "BR002", FileCount = 0 }
            };

            await context.FilingCounters.AddRangeAsync(counters);
            await context.SaveChangesAsync();
        }
    }
}
