using Microsoft.EntityFrameworkCore;
using PFTransactionalService.Domain.Aggregates;
using PFTransactionalService.Infrastructure.Persistence.EfCore;

namespace PFTransactionalService.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task SeedAsync(PFTransactionalDbContext context)
    {
        if (await context.PFAccumulations.AnyAsync())
            return;

        var accumulations = new[]
        {
            new PFAccumulation(100001, 500001, "PF1", 150000m, 75000m, 75000m, 1),
            new PFAccumulation(100002, 500002, "PF1", 280000m, 140000m, 140000m, 1),
            new PFAccumulation(100003, 500003, "PF2", 95000m, 47500m, 47500m, 2),
        };

        foreach (var a in accumulations)
            a.ClearDomainEvents();

        await context.PFAccumulations.AddRangeAsync(accumulations);
        await context.SaveChangesAsync();
    }
}
