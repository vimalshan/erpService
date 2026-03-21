using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using UnitService.Domain.Entities;
using UnitService.Infrastructure.Data;

namespace UnitService.Infrastructure.Seed;

public static class SeedData
{
    public static async Task InitializeAsync(UnitDbContext context, ILogger logger)
    {
        await context.Database.MigrateAsync();

        if (await context.EquipmentMasters.AnyAsync())
        {
            logger.LogInformation("Database already seeded.");
            return;
        }

        logger.LogInformation("Seeding database...");

        var equipment1 = EquipmentMaster.Create(1, "CNC Machine A", "MCH", "Machinery", 1);
        var equipment2 = EquipmentMaster.Create(2, "Forklift B", "VEH", "Vehicle", 1);
        var equipment3 = EquipmentMaster.Create(3, "Drill Press C", "MCH", "Machinery", 1);

        // Clear domain events from seed data (they shouldn't publish during seeding)
        equipment1.ClearDomainEvents();
        equipment2.ClearDomainEvents();
        equipment3.ClearDomainEvents();

        await context.EquipmentMasters.AddRangeAsync(equipment1, equipment2, equipment3);

        var category1 = CategoryMaster.Create("MCH", 1, "Machinery", 1);
        var category2 = CategoryMaster.Create("VEH", 2, "Vehicle", 1);

        await context.CategoryMasters.AddRangeAsync(category1, category2);

        var status1 = EquipmentStatus.Create(1, 1, "Active", "ACT", "Initial setup", null, 1);
        var status2 = EquipmentStatus.Create(2, 2, "Active", "ACT", "Initial setup", null, 1);
        status1.ClearDomainEvents();
        status2.ClearDomainEvents();

        await context.EquipmentStatuses.AddRangeAsync(status1, status2);

        await context.SaveChangesAsync();
        logger.LogInformation("Database seeded successfully.");
    }
}
