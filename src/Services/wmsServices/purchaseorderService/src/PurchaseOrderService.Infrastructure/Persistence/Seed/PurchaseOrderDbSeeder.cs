using Microsoft.EntityFrameworkCore;
using PurchaseOrderService.Domain.Entities;
using PurchaseOrderService.Domain.Enums;
using PurchaseOrderService.Infrastructure.Persistence;

namespace PurchaseOrderService.Infrastructure.Persistence.Seed;

public static class PurchaseOrderDbSeeder
{
    public static async Task SeedAsync(PurchaseOrderDbContext context)
    {
        if (await context.PurchaseOrders.AnyAsync())
            return;

        var po1 = PurchaseOrder.Create("PO-2024-0001", 1, 1, new DateTime(2024, 1, 15), new DateTime(2024, 2, 15), "Initial stationery order", "admin");
        po1.AddLine(1, 1, 100, 2.50m, "Blue ballpoint pens");
        po1.AddLine(2, 2, 50, 5.00m, "A4 notebooks");
        po1.AddLine(3, 3, 200, 0.10m, "Paper clips");

        var po2 = PurchaseOrder.Create("PO-2024-0002", 2, 1, new DateTime(2024, 2, 1), new DateTime(2024, 3, 1), "Office supplies restock", "admin");
        po2.AddLine(4, 1, 500, 0.05m, "Staples box");
        po2.AddLine(5, 2, 20, 15.00m, "Desk organizers");

        var po3 = PurchaseOrder.Create("PO-2024-0003", 1, 2, new DateTime(2024, 3, 1), null, "Warehouse B supplies", "admin");
        po3.AddLine(1, 1, 1000, 0.02m, "Envelopes");

        context.PurchaseOrders.AddRange(po1, po2, po3);
        await context.SaveChangesAsync();
    }
}
