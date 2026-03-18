using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TimesheetService.Domain.Entities;
using TimesheetService.Infrastructure.Data;

namespace TimesheetService.Infrastructure.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context     = scope.ServiceProvider.GetRequiredService<TimesheetDbContext>();
        var logger      = scope.ServiceProvider.GetRequiredService<ILogger<TimesheetDbContext>>();

        try
        {
            // Apply any pending migrations automatically on startup
            var pending = await context.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                logger.LogInformation("Applying {Count} pending EF Core migration(s)...", pending.Count());
                await context.Database.MigrateAsync();
                logger.LogInformation("Migrations applied successfully.");
            }
            else
            {
                logger.LogInformation("Database is up to date — no pending migrations.");
            }

            // Only seed if the table is completely empty
            if (await context.Timesheets.AnyAsync())
            {
                logger.LogInformation("Seed skipped — timesheet data already exists.");
                return;
            }

            logger.LogInformation("Seeding initial timesheet data...");

            var entries = BuildSeedData();
            context.Timesheets.AddRange(entries);
            // Suppress domain events on the seed path
            foreach (var e in entries) e.ClearDomainEvents();

            await context.SaveChangesAsync();
            logger.LogInformation("Seed complete — {Count} timesheet records inserted.", entries.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    // ── Sample data spanning three employees, two projects across March 2026 ──
    private static List<Timesheet> BuildSeedData()
    {
        var today  = new DateOnly(2026, 3, 15);
        var seeds  = new List<Timesheet>();

        // ── Employee 1001 — full working week, APPROVED ──────────────────────
        foreach (var offset in Enumerable.Range(1, 5))
        {
            var workDate = today.AddDays(-offset - 7);   // week before last
            var t = Timesheet.Create(
                employeeId:      1001,
                timesheetDate:   today,
                workDate:        workDate,
                startTime:       new TimeOnly(9, 0),
                endTime:         new TimeOnly(17, 30),
                totalHours:      8.5m,
                projectId:       100,
                taskId:          201,
                workDescription: $"Project Alpha — development work, day {offset}.",
                createdBy:       1001);
            t.Submit(1001);
            t.Approve(9001);
            seeds.Add(t);
        }

        // ── Employee 1001 — current week, SUBMITTED (pending approval) ────────
        foreach (var offset in Enumerable.Range(1, 3))
        {
            var workDate = today.AddDays(-offset);
            var t = Timesheet.Create(
                employeeId:      1001,
                timesheetDate:   today,
                workDate:        workDate,
                startTime:       new TimeOnly(9, 0),
                endTime:         new TimeOnly(17, 0),
                totalHours:      8.0m,
                projectId:       100,
                taskId:          202,
                workDescription: $"Project Alpha — testing work, day {offset}.",
                createdBy:       1001);
            t.Submit(1001);
            seeds.Add(t);
        }

        // ── Employee 1002 — DRAFT entries ─────────────────────────────────────
        foreach (var offset in Enumerable.Range(1, 3))
        {
            var workDate = today.AddDays(-offset);
            var t = Timesheet.Create(
                employeeId:      1002,
                timesheetDate:   today,
                workDate:        workDate,
                startTime:       new TimeOnly(8, 30),
                endTime:         new TimeOnly(16, 30),
                totalHours:      8.0m,
                projectId:       101,
                taskId:          301,
                workDescription: $"Project Beta — design work, day {offset}.",
                createdBy:       1002);
            seeds.Add(t);
        }

        // ── Employee 1003 — one REJECTED entry ───────────────────────────────
        {
            var t = Timesheet.Create(
                employeeId:      1003,
                timesheetDate:   today,
                workDate:        today.AddDays(-5),
                startTime:       new TimeOnly(9, 0),
                endTime:         new TimeOnly(13, 0),
                totalHours:      4.0m,
                projectId:       101,
                taskId:          302,
                workDescription: "Project Beta — half day, requirements review.",
                createdBy:       1003);
            t.Submit(1003);
            t.Reject(9001, "Hours do not match the attendance log. Please correct and resubmit.");
            seeds.Add(t);
        }

        // ── Employee 1003 — APPROVED entry ───────────────────────────────────
        {
            var t = Timesheet.Create(
                employeeId:      1003,
                timesheetDate:   today,
                workDate:        today.AddDays(-6),
                startTime:       new TimeOnly(9, 0),
                endTime:         new TimeOnly(18, 0),
                totalHours:      9.0m,
                projectId:       101,
                taskId:          302,
                workDescription: "Project Beta — client demo preparation.",
                createdBy:       1003);
            t.Submit(1003);
            t.Approve(9001);
            seeds.Add(t);
        }

        return seeds;
    }
}
