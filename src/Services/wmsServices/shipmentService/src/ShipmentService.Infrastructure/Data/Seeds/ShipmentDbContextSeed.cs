using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShipmentService.Domain.Entities;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Infrastructure.Data.Seeds;

public static class ShipmentDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ShipmentDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ShipmentDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.Shipments.AnyAsync())
            {
                logger.LogInformation("Seeding initial shipment data...");

                var shipment1 = Shipment.Create("SHP-2026-0001", 1, 1, ShipmentType.Outbound,
                    "Express", "FedEx", "TRK123456789", null, "system");
                shipment1.UpdateStatus(ShipmentStatus.Open, "Warehouse", "Shipment opened", "system");
                shipment1.AddPackage("PKG-001", 5.5m, 0.02m, "30x20x15", "PKG-TRK-001", "Electronics");

                var shipment2 = Shipment.Create("SHP-2026-0002", 2, 1, ShipmentType.Outbound,
                    "Standard", "UPS", "TRK987654321", null, "system");

                context.Shipments.AddRange(shipment1, shipment2);
                await context.SaveChangesAsync();
                logger.LogInformation("Shipment seed data applied.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
