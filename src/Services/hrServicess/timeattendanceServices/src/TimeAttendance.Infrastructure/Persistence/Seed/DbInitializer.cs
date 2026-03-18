using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimeAttendance.Domain.Entities;

namespace TimeAttendance.Infrastructure.Persistence.Seed;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TimeAttendanceDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<TimeAttendanceDbContext>>();

        try
        {
            logger.LogInformation("Applying database migrations...");
            await context.Database.MigrateAsync();

            await SeedAbsenteeismDetailsAsync(context);
            await SeedAbsenteeismMisAsync(context);
            logger.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    private static async Task SeedAbsenteeismDetailsAsync(TimeAttendanceDbContext context)
    {
        if (await context.AbsenteeismDetails.AnyAsync()) return;

        var seedData = new List<AbsenteeismDetail>
        {
            AbsenteeismDetail.Create(1001, 2025, 1, 5000, 250, "A", 101, 1, 2, 'M', 3, 5),
            AbsenteeismDetail.Create(1001, 2025, 2, 4800, 300, "A", 101, 1, 2, 'F', 3, 5),
            AbsenteeismDetail.Create(1002, 2025, 1, 3500, 175, "B", 102, 2, 3, 'M', 4, 6),
            AbsenteeismDetail.Create(1002, 2025, 2, 3400, 204, "B", 102, 2, 3, 'F', 4, 6),
            AbsenteeismDetail.Create(1003, 2025, 1, 2000, 80, "C", 103, 3, 1, 'M', 2, 4),
        };

        foreach (var item in seedData)
            item.ClearDomainEvents();

        await context.AbsenteeismDetails.AddRangeAsync(seedData);
        await context.SaveChangesAsync();
    }

    private static async Task SeedAbsenteeismMisAsync(TimeAttendanceDbContext context)
    {
        if (await context.AbsenteeismMisRecords.AnyAsync()) return;

        var seedData = new List<AbsenteeismMis>
        {
            AbsenteeismMis.Create(1001, 10, 201, 301, "A", "202501"),
            AbsenteeismMis.Create(1002, 10, 202, 302, "B", "202501"),
            AbsenteeismMis.Create(1003, 11, 203, 303, "C", "202501"),
        };

        foreach (var item in seedData)
        {
            item.UpdateLeaveData(22, 20, 4, 2, 160, 1, 0, 18, 0, 0, 0, 10);
            item.ClearDomainEvents();
        }

        await context.AbsenteeismMisRecords.AddRangeAsync(seedData);
        await context.SaveChangesAsync();
    }
}
