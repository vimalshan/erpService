using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TimeSheetService.Domain.Entities;
using TimeSheetService.Domain.ValueObjects;
using TimeSheetService.Infrastructure.Persistence;

namespace TimeSheetService.Infrastructure.Seed;

public static class TimeSheetDbContextSeed
{
    public static async Task SeedAsync(TimeSheetDbContext context, ILogger? logger = null)
    {
        try
        {
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            logger?.LogError(ex, "An error occurred while applying migrations.");
            throw;
        }

        if (await context.TimesheetEntries.AnyAsync()) 
        {
            logger?.LogInformation("Database already seeded — skipping");
            return;
        }

        logger?.LogInformation("Seeding timesheet data...");

        // Seed TC Projects (use Id=0 so SQL Server IDENTITY auto-assigns)
        var tcProjects = new[]
        {
            new TcProject(0, "ERP System Development", 1, DateTime.UtcNow.AddMonths(-6), 1, 'Y', 1000),
            new TcProject(0, "HR Portal Upgrade", 1, DateTime.UtcNow.AddMonths(-3), 1, 'Y', 1000),
            new TcProject(0, "Infrastructure Refresh", 2, DateTime.UtcNow.AddMonths(-1), 2, 'Y', 1000),
        };
        await context.TcProjects.AddRangeAsync(tcProjects);
        await context.SaveChangesAsync();

        // Seed Timesheet Entries (use Id=0 so SQL Server IDENTITY auto-assigns)
        var entries = new[]
        {
            new TimesheetEntry(0, 1001, DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(-1).AddHours(8),
                DateTime.UtcNow.AddDays(-1).AddHours(17),
                8, "Regular day", EntryType.Self, 1001),
            new TimesheetEntry(0, 1002, DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(-1).AddHours(9),
                DateTime.UtcNow.AddDays(-1).AddHours(18),
                8, "Regular day", EntryType.Self, 1002),
            new TimesheetEntry(0, 1001, DateTime.UtcNow.AddDays(-2),
                DateTime.UtcNow.AddDays(-2).AddHours(8),
                DateTime.UtcNow.AddDays(-2).AddHours(17),
                8, "Regular day", EntryType.Self, 1001),
            new TimesheetEntry(0, 1003, DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(-1).AddHours(8),
                DateTime.UtcNow.AddDays(-1).AddHours(16),
                7, "Short day", EntryType.Manual, 1000),
            new TimesheetEntry(0, 1004, DateTime.UtcNow.AddDays(-1),
                DateTime.UtcNow.AddDays(-1).AddHours(9),
                DateTime.UtcNow.AddDays(-1).AddHours(18),
                8, "Regular day", EntryType.Automatic, 1000),
        };

        await context.TimesheetEntries.AddRangeAsync(entries);
        await context.SaveChangesAsync();

        logger?.LogInformation("Seeded {Count} timesheet entries", entries.Length);
    }
}
