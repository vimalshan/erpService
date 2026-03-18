using Microsoft.EntityFrameworkCore;
using OtherService.Domain.Entities;

namespace OtherService.Infrastructure.Persistence;

/// <summary>
/// Seed initial data for development/testing.
/// </summary>
public static class DbContextSeeds
{
    public static async Task SeedAsync(OtherDbContext context)
    {
        // Only seed if database has no entries
        if (await context.LogDdCatDevDetails.AnyAsync())
            return;

        var entries = new List<LogDdCatDevDetail>
        {
            LogDdCatDevDetail.Create(
                appId: "USR001",
                appNum: 1,
                reqNum: 1001,
                qtnNum: 101,
                ansSrl: 1,
                entDat: DateTime.UtcNow,
                desc: "Advanced project management skills",
                need: "To lead cross-functional teams effectively"),

            LogDdCatDevDetail.Create(
                appId: "USR002",
                appNum: 2,
                reqNum: 1002,
                qtnNum: 102,
                ansSrl: 2,
                entDat: DateTime.UtcNow.AddDays(-1),
                desc: "Data analysis and visualization",
                need: "To make data-driven decisions"),

            LogDdCatDevDetail.Create(
                appId: "USR003",
                appNum: 3,
                reqNum: 1003,
                qtnNum: 103,
                ansSrl: 1,
                entDat: DateTime.UtcNow.AddDays(-7),
                desc: "Cloud architecture design",
                need: "To design scalable microservices"),
        };

        foreach (var entry in entries)
        {
            await context.LogDdCatDevDetails.AddAsync(entry);
        }

        await context.SaveChangesAsync();
    }
}
