using Microsoft.EntityFrameworkCore;
using RackingSystem.Domain.Entities;

namespace RackingSystem.Infrastructure.Persistence.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        // Apply any pending migrations
        await context.Database.MigrateAsync();

        // Seed only if no racks exist
        if (await context.Racks.AnyAsync()) return;

        var racks = new List<Rack>
        {
            Rack.Create(1, "RACK-A01", "PALLET", 2000),
            Rack.Create(1, "RACK-A02", "SHELVING", 500),
            Rack.Create(2, "RACK-B01", "CANTILEVER", 3000)
        };

        // Clear domain events before seeding to avoid publisher side-effects at startup
        racks.ForEach(r => r.ClearDomainEvents());
        await context.Racks.AddRangeAsync(racks);
        await context.SaveChangesAsync();

        // Shelves
        var shelves = new List<Shelf>
        {
            Shelf.Create(1, 1, 1, "RACK-A01-L1-P1", 50, 300),
            Shelf.Create(1, 2, 1, "RACK-A01-L2-P1", 50, 300),
            Shelf.Create(2, 1, 1, "RACK-A02-L1-P1", 30, 100)
        };
        await context.Shelves.AddRangeAsync(shelves);
        await context.SaveChangesAsync();

        // Bins
        var bins = new List<Bin>
        {
            Bin.Create(1, "BIN-A01-001", 1, "BC001", "STANDARD", 20, 100, 0.5m),
            Bin.Create(1, "BIN-A01-002", 1, "BC002", "STANDARD", 20, 100, 0.5m),
            Bin.Create(1, "BIN-A02-001", 2, "BC003", "OVERSIZE",  5,  200, 2.0m),
            Bin.Create(2, "BIN-B01-001", null, null, "FLOOR", 100, 1000, 5.0m)
        };
        bins.ForEach(b => b.ClearDomainEvents());
        await context.Bins.AddRangeAsync(bins);
        await context.SaveChangesAsync();
    }
}
