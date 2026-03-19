using Microsoft.EntityFrameworkCore;
using ProductionManagement.Domain.Entities;
using ProductionManagement.Infrastructure.Persistence;

namespace ProductionManagement.Infrastructure.Seed;

public static class SeedData
{
    public static async Task SeedAsync(ProductionManagementDbContext context)
    {
        if (await context.ProductionPlants.AnyAsync())
            return;

        // Seed Production Plants
        var plant1 = new ProductionPlant(1, "Main Manufacturing Plant", "New York", 1);
        var plant2 = new ProductionPlant(1, "Secondary Assembly Plant", "Chicago", 1);
        var plant3 = new ProductionPlant(2, "Electronics Plant", "San Francisco", 1);

        context.ProductionPlants.AddRange(plant1, plant2, plant3);
        await context.SaveChangesAsync();

        // Seed Norms
        var norm1 = new NormsMain(1001, DateTime.UtcNow.AddDays(-30));
        norm1.AddNormsMaster(1, 100, 200, 50);
        norm1.AddNormsMaster(2, 101, 201, 75);

        var norm2 = new NormsMain(1002, DateTime.UtcNow.AddDays(-15));
        norm2.AddNormsMaster(3, 102, 202, 60);

        context.NormsMain.AddRange(norm1, norm2);
        await context.SaveChangesAsync();
    }
}
