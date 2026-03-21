using Microsoft.EntityFrameworkCore;
using TourPlanService.Domain.Entities;
using TourPlanService.Infrastructure.Data;

namespace TourPlanService.Infrastructure.Data.Seed;

public static class TourPlanSeedData
{
    public static async Task SeedAsync(TourPlanDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        if (await context.TourPlans.AnyAsync()) return;

        var tourPlan1 = TourPlan.Create(
            tpId: "TP-2026-001",
            empSysId: "EMP001",
            startDate: DateTime.UtcNow.AddDays(7),
            purpose: "Client Meeting - Project Review",
            remarks: "Quarterly project review with ABC Corp",
            category: "DOM",
            bookInc: "Y",
            fromCityId: "MUM",
            fromCityName: "Mumbai",
            toCityId: "DEL",
            toCityName: "Delhi",
            supRemarks: "Important client visit",
            createdBy: "EMP001");

        var tourPlan2 = TourPlan.Create(
            tpId: "TP-2026-002",
            empSysId: "EMP002",
            startDate: DateTime.UtcNow.AddDays(14),
            purpose: "International Conference - Technology Summit",
            remarks: "Global tech conference participation",
            category: "INT",
            bookInc: "Y",
            fromCityId: "MUM",
            fromCityName: "Mumbai",
            toCityId: "NYC",
            toCityName: "New York",
            supRemarks: "Annual technology conference",
            createdBy: "EMP002");

        context.TourPlans.AddRange(tourPlan1, tourPlan2);
        await context.SaveChangesAsync();
    }
}
