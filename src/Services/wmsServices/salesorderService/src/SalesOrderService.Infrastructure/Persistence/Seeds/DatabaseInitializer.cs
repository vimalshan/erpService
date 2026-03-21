using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SalesOrderService.Infrastructure.Persistence.Seeds;

/// <summary>
/// Applies any pending EF migrations on startup and seeds reference data.
/// </summary>
public static class DatabaseInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db     = scope.ServiceProvider.GetRequiredService<SalesOrderDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<SalesOrderDbContext>>();

        try
        {
            if ((await db.Database.GetPendingMigrationsAsync()).Any())
            {
                logger.LogInformation("Applying pending migrations...");
                await db.Database.MigrateAsync();
                logger.LogInformation("Migrations applied.");
            }

            await SeedAsync(db, logger);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed.");
            throw;
        }
    }

    private static async Task SeedAsync(SalesOrderDbContext db, ILogger logger)
    {
        if (await db.SalesOrders.AnyAsync()) return;

        logger.LogInformation("Seeding initial sales order data...");

        var now  = DateTime.UtcNow;
        var today = DateOnly.FromDateTime(now);

        // Note: Customer and Warehouse records must already exist in their respective tables.
        // These seeds use placeholder IDs — adjust to match your actual data.
        var order = Domain.Entities.SalesOrder.Create(
            soNumber: "SO-00001",
            customerId: 1,
            warehouseId: 1,
            orderDate: today,
            requestedDate: today.AddDays(7),
            notes: "Seed order",
            createdBy: "system");

        order.AddLine(productId: 1, lineNumber: 1, quantityOrdered: 10, unitPrice: 25.00m, discount: 0);
        order.AddLine(productId: 2, lineNumber: 2, quantityOrdered: 5,  unitPrice: 49.99m, discount: 5.00m);
        order.ClearDomainEvents(); // no events during seed

        db.SalesOrders.Add(order);
        await db.SaveChangesAsync();
        logger.LogInformation("Seed data applied.");
    }
}
