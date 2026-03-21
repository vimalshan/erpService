using MasterDataService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Infrastructure.Data.Seed;

public static class MasterDataSeeder
{
    public static async Task SeedAsync(MasterDataDbContext context, ILogger logger)
    {
        try
        {
            await context.Database.MigrateAsync();

            if (!await context.GuestHouses.AnyAsync())
            {
                var guestHouses = new List<GuestHouse>
                {
                    new(1001, "Main Guest House", 500),
                    new(1002, "Executive Guest House", 1200),
                    new(1003, "Transit Guest House", 300),
                };
                context.GuestHouses.AddRange(guestHouses);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} guest houses", guestHouses.Count);
            }

            if (!await context.Areas.AnyAsync())
            {
                var areas = new List<Area>
                {
                    new(1, "North Zone"),
                    new(2, "South Zone"),
                    new(3, "East Zone"),
                    new(4, "West Zone"),
                };
                context.Areas.AddRange(areas);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} areas", areas.Count);
            }

            if (!await context.Routes.AnyAsync())
            {
                var routes = new List<Domain.Entities.Route>
                {
                    new(1, "Delhi - Mumbai"),
                    new(2, "Mumbai - Chennai"),
                    new(3, "Delhi - Kolkata"),
                    new(4, "Chennai - Bangalore"),
                };
                context.Routes.AddRange(routes);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} routes", routes.Count);
            }

            if (!await context.TaxSlabs.AnyAsync())
            {
                var taxSlabs = new List<TaxSlab>
                {
                    new("GST", DateTime.UtcNow.AddYears(-1), null, 18m, 1001),
                    new("IGST", DateTime.UtcNow.AddYears(-1), null, 12m, 1002),
                };
                context.TaxSlabs.AddRange(taxSlabs);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} tax slabs", taxSlabs.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding master data");
            throw;
        }
    }
}
