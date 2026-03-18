using BookingService.Domain.Entities;
using BookingService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookingService.Infrastructure.Persistence.Seed;

public static class BookingDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BookingDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<BookingDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.BookMains.AnyAsync())
            {
                logger.LogInformation("Seeding initial booking data...");

                var booking1 = BookMain.Create("BK-2026-0001", "Annual Conference Booking", "LOC001", new DateTime(2026, 4, 15), 1);
                var booking2 = BookMain.Create("BK-2026-0002", "Team Building Workshop", "LOC002", new DateTime(2026, 5, 20), 1);

                booking1.AddRecord("LOC001", "Main hall setup required - 200 seats.", 1);
                booking1.AddAttendee(1001, 1);
                booking1.AddAttendee(1002, 1);

                booking2.AddRecord("LOC002", "Workshop room A, projector required.", 1);
                booking2.AddAttendee(2001, 1);

                await context.BookMains.AddRangeAsync(booking1, booking2);
                await context.SaveChangesAsync();

                logger.LogInformation("Seed data applied successfully.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }
}
