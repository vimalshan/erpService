using CanteenUnit.Domain.Entities;
using CanteenUnit.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CanteenUnit.Infrastructure.Persistence.Seeders;

public static class DataSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.GenCounters.AnyAsync())
            {
                context.GenCounters.AddRange(
                    GenCounter.Create("CUN", 0, "Canteen Unit Number"),
                    GenCounter.Create("CAN", 0, "Canteen Number"),
                    GenCounter.Create("ACC", 0, "Access Number"));
                await context.SaveChangesAsync();
                logger.LogInformation("GenCounter seed data applied.");
            }

            if (!await context.CanteenUnitMasters.AnyAsync())
            {
                context.CanteenUnitMasters.AddRange(
                    CanteenUnitMaster.Create(1001, "Main Canteen Unit", "MCU-001", 5000, 100, 1, 1001),
                    CanteenUnitMaster.Create(1002, "North Wing Unit", "NWU-002", 3000, 50, 2, 1002),
                    CanteenUnitMaster.Create(1003, "South Wing Unit", "SWU-003", 4000, 75, 3, 1003));
                await context.SaveChangesAsync();
                logger.LogInformation("CanteenUnitMaster seed data applied.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
