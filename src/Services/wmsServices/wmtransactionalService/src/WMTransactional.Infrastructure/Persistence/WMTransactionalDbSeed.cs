using Microsoft.EntityFrameworkCore;
using WMTransactional.Domain.Entities;

namespace WMTransactional.Infrastructure.Persistence;

public static class WMTransactionalDbSeed
{
    public static async Task SeedAsync(WMTransactionalDbContext context)
    {
        await context.Database.MigrateAsync();

        if (await context.PurchaseOrders.AnyAsync()) return;

        // Seed Purchase Orders
        var po1 = new PurchaseOrder("PO-2026-001", supplierId: 1, expectedDate: DateTime.UtcNow.AddDays(7), notes: "Initial order", createdBy: "system");
        po1.AddLine(productId: 1, lineNumber: 1, quantityOrdered: 100, unitPrice: 25.50m, notes: null);
        po1.AddLine(productId: 2, lineNumber: 2, quantityOrdered: 200, unitPrice: 12.75m, notes: null);
        po1.Confirm();

        var po2 = new PurchaseOrder("PO-2026-002", supplierId: 2, expectedDate: DateTime.UtcNow.AddDays(14), notes: "Bulk order", createdBy: "system");
        po2.AddLine(productId: 3, lineNumber: 1, quantityOrdered: 500, unitPrice: 8.00m, notes: null);
        po2.AddLine(productId: 4, lineNumber: 2, quantityOrdered: 50, unitPrice: 150.00m, notes: null);

        await context.PurchaseOrders.AddRangeAsync(po1, po2);

        // Seed Sales Orders
        var so1 = new SalesOrder("SO-2026-001", customerId: 1, requestedDate: DateTime.UtcNow.AddDays(3), notes: "Urgent order", createdBy: "system");
        so1.AddLine(productId: 1, lineNumber: 1, quantityOrdered: 10, unitPrice: 35.00m, notes: null);
        so1.AddLine(productId: 2, lineNumber: 2, quantityOrdered: 25, unitPrice: 18.50m, notes: null);
        so1.Confirm();

        var so2 = new SalesOrder("SO-2026-002", customerId: 2, requestedDate: DateTime.UtcNow.AddDays(10), notes: "Standard order", createdBy: "system");
        so2.AddLine(productId: 3, lineNumber: 1, quantityOrdered: 100, unitPrice: 12.00m, notes: null);

        await context.SalesOrders.AddRangeAsync(so1, so2);

        await context.SaveChangesAsync();
    }
}
