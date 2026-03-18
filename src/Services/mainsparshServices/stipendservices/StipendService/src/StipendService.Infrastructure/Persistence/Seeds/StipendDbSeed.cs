using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using StipendService.Domain.Entities;

namespace StipendService.Infrastructure.Persistence.Seeds;

public static class StipendDbSeed
{
    public static async Task SeedAsync(StipendDbContext context)
    {
        if (!await context.StipendMasters.AnyAsync())
        {
            var seeds = new[]
            {
                StipendMaster.Create(1, 1, 37000m, 2000m, new DateTime(2026, 1, 1), null, 1),
                StipendMaster.Create(1, 2, 42000m, 2500m, new DateTime(2026, 1, 1), null, 1),
                StipendMaster.Create(2, 1, 35000m, 1800m, new DateTime(2026, 1, 1), null, 1),
                StipendMaster.Create(2, 2, 40000m, 2200m, new DateTime(2026, 1, 1), null, 1),
                StipendMaster.Create(3, 1, 38000m, 1900m, new DateTime(2026, 1, 1), null, 1),
            };

            foreach (var seed in seeds)
                seed.ClearDomainEvents();

            await context.StipendMasters.AddRangeAsync(seeds);
            await context.SaveChangesAsync();
        }
    }
}
