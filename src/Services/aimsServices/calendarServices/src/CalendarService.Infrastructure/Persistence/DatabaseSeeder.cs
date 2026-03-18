using CalendarService.Domain.Entities;
using CalendarService.Domain.ValueObjects;
using CalendarService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CalendarService.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CalendarDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<CalendarDbContext>>();

        try
        {
            await db.Database.MigrateAsync();

            if (!await db.ShiftMasters.AnyAsync())
            {
                var morning = ShiftMaster.Create(1, "MOR", "Morning Shift", new TimeOnly(8, 0), new TimeOnly(16, 0), 1);
                var evening = ShiftMaster.Create(2, "EVE", "Evening Shift", new TimeOnly(16, 0), new TimeOnly(0, 0), 1);
                var night   = ShiftMaster.Create(3, "NGT", "Night Shift",   new TimeOnly(0, 0), new TimeOnly(8, 0), 1);
                db.ShiftMasters.AddRange(morning, evening, night);
                await db.SaveChangesAsync();
                logger.LogInformation("Seeded shifts");
            }

            if (!await db.HolidayMasters.AnyAsync())
            {
                int year = DateTime.UtcNow.Year;
                var holidays = new[]
                {
                    HolidayMaster.Create(1, new DateTime(year, 1, 1),  "New Year's Day",       HolidayType.National, 1),
                    HolidayMaster.Create(2, new DateTime(year, 8, 15), "Independence Day",     HolidayType.National, 1),
                    HolidayMaster.Create(3, new DateTime(year, 12, 25),"Christmas Day",        HolidayType.National, 1),
                };
                db.HolidayMasters.AddRange(holidays);
                await db.SaveChangesAsync();
                logger.LogInformation("Seeded holidays");
            }

            if (!await db.CalendarMasters.AnyAsync())
            {
                var cal = CalendarMaster.Create(1, "Default Calendar", 1, new DateTime(DateTime.UtcNow.Year, 1, 1), 1);
                db.CalendarMasters.Add(cal);
                await db.SaveChangesAsync();
                logger.LogInformation("Seeded calendars");
            }

            if (!await db.PatternMasters.AnyAsync())
            {
                var pattern = PatternMaster.Create(1, "Standard 5-Day", 7, 1, "Mon-Fri standard work week");
                db.PatternMasters.Add(pattern);
                await db.SaveChangesAsync();
                logger.LogInformation("Seeded patterns");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
}
