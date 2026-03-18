using FillingOperationService.Domain.Entities;
using FillingOperationService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FillingOperationService.Infrastructure.Persistence.Seed;

public static class FillingOperationsSeedData
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FillingOperationsDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<FillingOperationsDbContext>>();

        if (await context.FillingPlants.AnyAsync()) return;

        logger.LogInformation("Seeding Filling Operations data...");

        // ── 1. FILLING PLANTS ────────────────────────────────────────────────────
        var plant1 = FillingPlant.Create(10, "North Plant",   "Zone-N", 1);
        var plant2 = FillingPlant.Create(10, "South Plant",   "Zone-S", 1);
        var plant3 = FillingPlant.Create(20, "East Plant",    "Zone-E", 1);

        await context.FillingPlants.AddRangeAsync(plant1, plant2, plant3);
        await context.SaveChangesAsync();

        // ── 2. FILLING LINES ─────────────────────────────────────────────────────
        // packageTypeId: 1=Bottle, 2=Can, 3=Pouch
        var p1Line1 = FillingLine.Create(plant1.FillingPlantId, "NP-Line-01", 6, 1, 1);
        var p1Line2 = FillingLine.Create(plant1.FillingPlantId, "NP-Line-02", 4, 2, 1);
        var p1Line3 = FillingLine.Create(plant1.FillingPlantId, "NP-Line-03", 8, 1, 1);

        var p2Line1 = FillingLine.Create(plant2.FillingPlantId, "SP-Line-01", 6, 1, 1);
        var p2Line2 = FillingLine.Create(plant2.FillingPlantId, "SP-Line-02", 4, 3, 1);

        var p3Line1 = FillingLine.Create(plant3.FillingPlantId, "EP-Line-01", 10, 2, 1);
        var p3Line2 = FillingLine.Create(plant3.FillingPlantId, "EP-Line-02", 6,  1, 1);

        await context.FillingLines.AddRangeAsync(p1Line1, p1Line2, p1Line3, p2Line1, p2Line2, p3Line1, p3Line2);
        await context.SaveChangesAsync();

        // ── 3. FILLING POINT GROUPS ──────────────────────────────────────────────
        // exclusiveUse: null=shared, 1=exclusive
        var grp1  = FillingPointGroup.Create(p1Line1.FillingLineId, "NP-L1-GrpA", 3, null, 1);
        var grp2  = FillingPointGroup.Create(p1Line1.FillingLineId, "NP-L1-GrpB", 3, 1,    1);
        var grp3  = FillingPointGroup.Create(p1Line2.FillingLineId, "NP-L2-GrpA", 2, null, 1);
        var grp4  = FillingPointGroup.Create(p1Line2.FillingLineId, "NP-L2-GrpB", 2, 1,    1);
        var grp5  = FillingPointGroup.Create(p1Line3.FillingLineId, "NP-L3-GrpA", 4, null, 1);
        var grp6  = FillingPointGroup.Create(p2Line1.FillingLineId, "SP-L1-GrpA", 3, null, 1);
        var grp7  = FillingPointGroup.Create(p2Line2.FillingLineId, "SP-L2-GrpA", 4, null, 1);
        var grp8  = FillingPointGroup.Create(p3Line1.FillingLineId, "EP-L1-GrpA", 5, null, 1);
        var grp9  = FillingPointGroup.Create(p3Line2.FillingLineId, "EP-L2-GrpA", 3, 1,    1);

        await context.FillingPointGroups.AddRangeAsync(grp1, grp2, grp3, grp4, grp5, grp6, grp7, grp8, grp9);
        await context.SaveChangesAsync();

        // ── 4. FILLING CAPACITIES ────────────────────────────────────────────────
        // (groupId, mainProductId, packageTypeId, itemCapacityId, capacityPerShift, usagePriority, createdBy)
        // Products: 101=WaterStill, 102=WaterSparkling, 103=JuiceOrange, 104=JuiceMango
        // Package:  1=Bottle500ml, 2=Can330ml, 3=Pouch200ml
        // ItemCap:  201=Std250, 202=Std300, 203=Std480, 204=Std500
        var caps = new[]
        {
            FillingCapacity.Create(grp1.FillingPointGroupId,  101, 1, 201, 480, 1, 1),
            FillingCapacity.Create(grp1.FillingPointGroupId,  103, 1, 202, 360, 2, 1),
            FillingCapacity.Create(grp2.FillingPointGroupId,  101, 1, 203, 500, 1, 1),
            FillingCapacity.Create(grp2.FillingPointGroupId,  102, 1, 204, 420, 2, 1),
            FillingCapacity.Create(grp3.FillingPointGroupId,  102, 2, 201, 600, 1, 1),
            FillingCapacity.Create(grp4.FillingPointGroupId,  103, 2, 202, 540, 1, 1),
            FillingCapacity.Create(grp5.FillingPointGroupId,  104, 1, 203, 400, 1, 1),
            FillingCapacity.Create(grp6.FillingPointGroupId,  101, 1, 201, 480, 1, 1),
            FillingCapacity.Create(grp6.FillingPointGroupId,  104, 1, 203, 380, 2, 1),
            FillingCapacity.Create(grp7.FillingPointGroupId,  103, 3, 202, 320, 1, 1),
            FillingCapacity.Create(grp8.FillingPointGroupId,  102, 2, 204, 660, 1, 1),
            FillingCapacity.Create(grp9.FillingPointGroupId,  101, 1, 201, 500, 1, 1),
        };
        await context.FillingCapacities.AddRangeAsync(caps);
        await context.SaveChangesAsync();

        // ── 5. FILLING LINE PRODUCT MAPS ─────────────────────────────────────────
        var productMaps = new[]
        {
            FillingLineProductMap.Create(p1Line1.FillingLineId, 101, 1),
            FillingLineProductMap.Create(p1Line1.FillingLineId, 103, 1),
            FillingLineProductMap.Create(p1Line2.FillingLineId, 102, 1),
            FillingLineProductMap.Create(p1Line2.FillingLineId, 103, 1),
            FillingLineProductMap.Create(p1Line3.FillingLineId, 104, 1),
            FillingLineProductMap.Create(p2Line1.FillingLineId, 101, 1),
            FillingLineProductMap.Create(p2Line1.FillingLineId, 104, 1),
            FillingLineProductMap.Create(p2Line2.FillingLineId, 103, 1),
            FillingLineProductMap.Create(p3Line1.FillingLineId, 102, 1),
            FillingLineProductMap.Create(p3Line2.FillingLineId, 101, 1),
        };
        await context.FillingLineProductMaps.AddRangeAsync(productMaps);
        await context.SaveChangesAsync();

        // ── 6. FL SWITCHOVER TIMES ───────────────────────────────────────────────
        // Time (hours) to switch between products on the same line
        var switchovers = new[]
        {
            FlSwitchoverTime.Create(p1Line1.FillingLineId, 101, 103, 2, 1),
            FlSwitchoverTime.Create(p1Line1.FillingLineId, 103, 101, 2, 1),
            FlSwitchoverTime.Create(p1Line2.FillingLineId, 102, 103, 3, 1),
            FlSwitchoverTime.Create(p1Line2.FillingLineId, 103, 102, 3, 1),
            FlSwitchoverTime.Create(p2Line1.FillingLineId, 101, 104, 2, 1),
            FlSwitchoverTime.Create(p3Line1.FillingLineId, 102, 101, 1, 1),
        };
        await context.FlSwitchoverTimes.AddRangeAsync(switchovers);
        await context.SaveChangesAsync();

        // ── 7. FL WORKING SHIFTS ─────────────────────────────────────────────────
        // Shift codes: A=Morning, B=Afternoon, C=Night
        var baseDate = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var shifts = new[]
        {
            FlWorkingShift.Create((decimal)p1Line1.FillingLineId, 'A', baseDate),
            FlWorkingShift.Create((decimal)p1Line1.FillingLineId, 'B', baseDate),
            FlWorkingShift.Create((decimal)p1Line1.FillingLineId, 'C', baseDate),
            FlWorkingShift.Create((decimal)p1Line2.FillingLineId, 'A', baseDate),
            FlWorkingShift.Create((decimal)p1Line2.FillingLineId, 'B', baseDate),
            FlWorkingShift.Create((decimal)p2Line1.FillingLineId, 'A', baseDate),
            FlWorkingShift.Create((decimal)p2Line1.FillingLineId, 'B', baseDate),
            FlWorkingShift.Create((decimal)p3Line1.FillingLineId, 'A', baseDate),
        };
        await context.FlWorkingShifts.AddRangeAsync(shifts);
        await context.SaveChangesAsync();

        // ── 8. FPG DOWNTIMES ─────────────────────────────────────────────────────
        // DowntimeType: PLANNED, UNPLANNED
        var downtimes = new[]
        {
            FpgDowntime.Create(grp1.FillingPointGroupId,
                new DateTime(2025, 2, 10,  8, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 2, 10, 10, 0, 0, DateTimeKind.Utc), "2", "PLANNED",   1),
            FpgDowntime.Create(grp3.FillingPointGroupId,
                new DateTime(2025, 3,  5, 14, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 3,  5, 15, 30, 0, DateTimeKind.Utc), "1", "UNPLANNED", 1),
            FpgDowntime.Create(grp6.FillingPointGroupId,
                new DateTime(2025, 4, 20,  0, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 4, 20,  6, 0, 0, DateTimeKind.Utc), "3", "PLANNED",   1),
            FpgDowntime.Create(grp8.FillingPointGroupId,
                new DateTime(2025, 5, 15, 10, 0, 0, DateTimeKind.Utc),
                new DateTime(2025, 5, 15, 11, 0, 0, DateTimeKind.Utc), "2", "UNPLANNED", 1),
        };
        await context.FpgDowntimes.AddRangeAsync(downtimes);
        await context.SaveChangesAsync();

        // ── 9. PLAN DEVIATIONS ───────────────────────────────────────────────────
        var deviations = new[]
        {
            PlanDeviation.Create(new DateTime(2025, 2, 10, 0, 0, 0, DateTimeKind.Utc), p1Line1.FillingLineId, 101, "Equipment breakdown on filling point 2"),
            PlanDeviation.Create(new DateTime(2025, 3,  5, 0, 0, 0, DateTimeKind.Utc), p1Line2.FillingLineId, 103, "Raw material shortage — juice concentrate"),
            PlanDeviation.Create(new DateTime(2025, 4, 20, 0, 0, 0, DateTimeKind.Utc), p2Line1.FillingLineId, 104, "Planned maintenance window overrun by 2h"),
            PlanDeviation.Create(new DateTime(2025, 5, 15, 0, 0, 0, DateTimeKind.Utc), p3Line1.FillingLineId, 102, "Unplanned conveyance jam at discharge point"),
        };
        await context.PlanDeviations.AddRangeAsync(deviations);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeding complete: 3 plants, 7 lines, 9 groups, 12 capacities, 10 product maps, 6 switchovers, 8 shifts, 4 downtimes, 4 deviations.");
    }
}
