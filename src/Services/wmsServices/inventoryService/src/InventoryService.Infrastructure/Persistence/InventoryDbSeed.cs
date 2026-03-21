using Microsoft.EntityFrameworkCore;

namespace InventoryService.Infrastructure.Persistence;

public static class InventoryDbSeed
{
    public static async Task SeedAsync(InventoryDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.StockLevels.AnyAsync()) return;

        var stockLevels = new[]
        {
            new Domain.Entities.StockLevel(productId: 1, warehouseId: 1, binId: 1, quantityOnHand: 100),
            new Domain.Entities.StockLevel(productId: 2, warehouseId: 1, binId: 2, quantityOnHand: 250),
            new Domain.Entities.StockLevel(productId: 3, warehouseId: 1, binId: 3, quantityOnHand: 50),
            new Domain.Entities.StockLevel(productId: 1, warehouseId: 2, binId: 4, quantityOnHand: 75),
            new Domain.Entities.StockLevel(productId: 4, warehouseId: 2, binId: 5, quantityOnHand: 300),
        };

        await context.StockLevels.AddRangeAsync(stockLevels);

        var transactions = new[]
        {
            new Domain.Entities.InventoryTransaction(1, 1, 1, "RECEIPT", 100, createdBy: "system", comments: "Initial stock"),
            new Domain.Entities.InventoryTransaction(2, 1, 2, "RECEIPT", 250, createdBy: "system", comments: "Initial stock"),
            new Domain.Entities.InventoryTransaction(3, 1, 3, "RECEIPT", 50, createdBy: "system", comments: "Initial stock"),
            new Domain.Entities.InventoryTransaction(1, 2, 4, "RECEIPT", 75, createdBy: "system", comments: "Initial stock"),
            new Domain.Entities.InventoryTransaction(4, 2, 5, "RECEIPT", 300, createdBy: "system", comments: "Initial stock"),
        };

        await context.InventoryTransactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync();
    }
}
