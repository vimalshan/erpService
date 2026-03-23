using Stationery.Domain.Entities;
using Stationery.Infrastructure.Persistence;

namespace Stationery.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(StationeryDbContext context)
    {
        await SeedItemsAsync(context);
        await SeedBudgetsAsync(context);
        await SeedApproversAsync(context);
    }

    private static async Task SeedItemsAsync(StationeryDbContext context)
    {
        if (context.StationaryMasters.Any()) return;

        var items = new List<StationaryMaster>
        {
            new() { CatId = 1, LocId = 1, Description = "A4 Paper (500 sheets)", UomId = 1, Make = "Generic", PricePerUnit = 250, ReorderLevel = 100, VmId = 1, Closed = "N", OpeningStock = 1000, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 1, LocId = 1, Description = "Blue Ballpoint Pen", UomId = 2, Make = "Pilot", PricePerUnit = 15, ReorderLevel = 50, VmId = 1, Closed = "N", OpeningStock = 500, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 1, LocId = 1, Description = "Black Ballpoint Pen", UomId = 2, Make = "Pilot", PricePerUnit = 15, ReorderLevel = 50, VmId = 1, Closed = "N", OpeningStock = 45, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 2, LocId = 1, Description = "Stapler", UomId = 3, Make = "Kangaro", PricePerUnit = 180, ReorderLevel = 10, VmId = 2, Closed = "N", OpeningStock = 30, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 2, LocId = 1, Description = "Stapler Pins (100 pcs)", UomId = 1, Make = "Kangaro", PricePerUnit = 20, ReorderLevel = 20, VmId = 2, Closed = "N", OpeningStock = 8, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 3, LocId = 1, Description = "Sticky Notes (100 sheets)", UomId = 1, Make = "3M", PricePerUnit = 60, ReorderLevel = 15, VmId = 3, Closed = "N", OpeningStock = 75, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 3, LocId = 1, Description = "Highlighter Pen", UomId = 2, Make = "Camlin", PricePerUnit = 30, ReorderLevel = 20, VmId = 3, Closed = "N", OpeningStock = 150, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { CatId = 4, LocId = 1, Description = "Printer Ink Cartridge (Black)", UomId = 3, Make = "HP", PricePerUnit = 1200, ReorderLevel = 5, VmId = 4, Closed = "N", OpeningStock = 12, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
        };

        context.StationaryMasters.AddRange(items);
        await context.SaveChangesAsync();
    }

    private static async Task SeedBudgetsAsync(StationeryDbContext context)
    {
        if (context.DeptBudgets.Any()) return;

        var deptBudgets = new List<DeptBudget>
        {
            new() { LocId = 1, UnitCode = "HO ", DeptId = 100, FinYearId = 2026, BudgetAmount = 50000, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { LocId = 1, UnitCode = "HO ", DeptId = 101, FinYearId = 2026, BudgetAmount = 30000, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { LocId = 1, UnitCode = "HO ", DeptId = 102, FinYearId = 2026, BudgetAmount = 25000, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
        };

        var unitBudgets = new List<UnitBudget>
        {
            new() { LocId = 1, UnitCode = "HO ", FinYearId = 2026, BudgetAmount = 200000, UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
        };

        context.DeptBudgets.AddRange(deptBudgets);
        context.UnitBudgets.AddRange(unitBudgets);
        await context.SaveChangesAsync();
    }

    private static async Task SeedApproversAsync(StationeryDbContext context)
    {
        if (context.DeptApprovers.Any()) return;

        var deptApprovers = new List<DeptApprover>
        {
            new() { LocationId = 1, UnitCode = "HO ", DeptId = 100, EmpSysId = 201, Type = "A", EffectiveDate = new DateTime(2026, 1, 1), UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { LocationId = 1, UnitCode = "HO ", DeptId = 100, EmpSysId = 202, Type = "I", EffectiveDate = new DateTime(2026, 1, 1), UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
            new() { LocationId = 1, UnitCode = "HO ", DeptId = 101, EmpSysId = 203, Type = "A", EffectiveDate = new DateTime(2026, 1, 1), UpdatedBy = 1, UpdatedOn = DateTime.UtcNow },
        };

        var locationAdmins = new List<LocationAdmin>
        {
            new() { LocationId = 1, EmpSysId = 301, EffectiveDate = new DateTime(2026, 1, 1), UpdatedBy = 1, UpdatedOn = DateTime.UtcNow }
        };

        context.DeptApprovers.AddRange(deptApprovers);
        context.LocationAdmins.AddRange(locationAdmins);
        await context.SaveChangesAsync();
    }
}
