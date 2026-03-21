using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WarehouseStructure.Domain.Entities;
using WarehouseStructure.Infrastructure.Persistence;

namespace WarehouseStructure.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<WarehouseStructureDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WarehouseStructureDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully.");

            if (!await context.Warehouses.AnyAsync())
            {
                var warehouses = GetSeedWarehouses();
                await context.Warehouses.AddRangeAsync(warehouses);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} warehouses.", warehouses.Count);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }

    private static List<Warehouse> GetSeedWarehouses()
    {
        return new List<Warehouse>
        {
            new()
            {
                Code = "WH-MAIN",
                Name = "Main Distribution Center",
                AddressLine = "100 Industrial Blvd",
                City = "Chicago",
                State = "IL",
                Country = "USA",
                PostalCode = "60601",
                Phone = "312-555-0100",
                Email = "main@warehouse.com",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new()
            {
                Code = "WH-EAST",
                Name = "East Coast Warehouse",
                AddressLine = "200 Logistics Ave",
                City = "Newark",
                State = "NJ",
                Country = "USA",
                PostalCode = "07102",
                Phone = "973-555-0200",
                Email = "east@warehouse.com",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            },
            new()
            {
                Code = "WH-WEST",
                Name = "West Coast Warehouse",
                AddressLine = "300 Pacific Way",
                City = "Los Angeles",
                State = "CA",
                Country = "USA",
                PostalCode = "90001",
                Phone = "213-555-0300",
                Email = "west@warehouse.com",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            }
        };
    }
}
