using ItemMasterService.Domain.Entities;
using ItemMasterService.Infrastructure.Persistence.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ItemMasterService.Infrastructure.Persistence.Seed;

public static class SeedData
{
    public static async Task SeedAsync(ItemMasterDbContext db, ILogger logger)
    {
        if (await db.CanteenItemMasters.AnyAsync()) return;

        logger.LogInformation("[Seed] Seeding initial canteen item data...");

        var items = new[]
        {
            CanteenItemMaster.Create(1001, 1, "Rice Meal", "F", "RICE01", "system"),
            CanteenItemMaster.Create(1001, 2, "Chicken Curry", "F", "CHKN01", "system"),
            CanteenItemMaster.Create(1001, 3, "Mineral Water", "B", "WATER1", "system"),
            CanteenItemMaster.Create(1001, 4, "Orange Juice", "B", "OJUIC1", "system"),
            CanteenItemMaster.Create(1001, 5, "Bread Roll", "S", "BREAD1", "system"),
        };

        foreach (var item in items)
            item.ClearDomainEvents();

        await db.CanteenItemMasters.AddRangeAsync(items);

        var prices = new[]
        {
            CanteenItemPriceMaster.Create(1001, 1, 25, 50, DateTime.UtcNow.AddMonths(-6), "system"),
            CanteenItemPriceMaster.Create(1001, 2, 40, 60, DateTime.UtcNow.AddMonths(-6), "system"),
            CanteenItemPriceMaster.Create(1001, 3, 10, 5, DateTime.UtcNow.AddMonths(-6), "system"),
        };

        await db.CanteenItemPriceMasters.AddRangeAsync(prices);

        await db.SaveChangesAsync();
        logger.LogInformation("[Seed] Seed complete.");
    }
}
