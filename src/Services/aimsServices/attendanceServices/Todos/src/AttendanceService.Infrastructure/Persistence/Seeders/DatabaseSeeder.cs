using AttendanceService.Domain.Entities;
using AttendanceService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AttendanceService.Infrastructure.Persistence.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context, ILogger logger)
    {
        await context.Database.MigrateAsync();

        if (!await context.AttendanceBatches.AnyAsync())
        {
            var batch = AttendanceBatch.Create(1, 1, 1, 2026, 2026, 1);
            batch.Close(1);
            context.AttendanceBatches.Add(batch);
        }

        if (!await context.SwipeRawPunches.AnyAsync())
        {
            var punch1 = SwipeRawPunch.Create(1, 1001, DateTime.UtcNow.AddHours(-8), "G01", "I");
            var punch2 = SwipeRawPunch.Create(2, 1001, DateTime.UtcNow.AddHours(-1), "G01", "O");
            context.SwipeRawPunches.AddRange(punch1, punch2);
        }

        if (!await context.AttendanceOvertimes.AnyAsync())
        {
            var ot = AttendanceOvertime.Create(1, 1001, DateTime.UtcNow.Date, 2.5m, "REGULAR", 1);
            context.AttendanceOvertimes.Add(ot);
        }

        await context.SaveChangesAsync();
        logger.LogInformation("Seed data applied.");
    }
}
