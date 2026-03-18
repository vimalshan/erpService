using DealTicketing.Domain.Entities;
using DealTicketing.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DealTicketing.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        await using var scope = services.CreateAsyncScope();
        var db = scope.ServiceProvider.GetRequiredService<DealTicketingDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DealTicketingDbContext>>();

        try
        {
            await db.Database.MigrateAsync();
            await SeedDataAsync(db);
            logger.LogInformation("Database seeded successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred seeding the database.");
        }
    }

    private static async Task SeedDataAsync(DealTicketingDbContext db)
    {
        // Seed LOV Master
        if (!await db.LovMasters.AnyAsync())
        {
            db.LovMasters.AddRange(
                new LovMaster(1, "001", "FX Forward"),
                new LovMaster(2, "001", "FX Option"),
                new LovMaster(3, "001", "Interest Rate Swap"),
                new LovMaster(4, "001", "Cross Currency Swap"),
                new LovMaster(5, "002", "Hedging"),
                new LovMaster(6, "002", "Speculative"),
                new LovMaster(7, "003", "FX"),
                new LovMaster(8, "003", "Derivatives"),
                new LovMaster(9, "003", "Swaps"),
                new LovMaster(10, "004", "Call"),
                new LovMaster(11, "004", "Put"),
                new LovMaster(12, "005", "LIBOR"),
                new LovMaster(13, "005", "SOFR"),
                new LovMaster(14, "005", "EURIBOR")
            );
            await db.SaveChangesAsync();
        }

        // Seed Category Master
        if (!await db.CategoryMasters.AnyAsync())
        {
            db.CategoryMasters.AddRange(
                new CategoryMaster(1, "FX Spot & Forward", 'F', 1),
                new CategoryMaster(2, "FX Options", 'F', 1),
                new CategoryMaster(3, "Interest Rate Derivatives", 'D', 1),
                new CategoryMaster(4, "Cross Currency Swaps", 'S', 1),
                new CategoryMaster(5, "Interest Rate Swaps", 'S', 1)
            );
            await db.SaveChangesAsync();
        }

        // Seed Banks
        if (!await db.Banks.AnyAsync())
        {
            db.Banks.AddRange(
                new Bank(1, "Standard Chartered Bank", DateTime.UtcNow.AddYears(-10), 1),
                new Bank(2, "HSBC Holdings", DateTime.UtcNow.AddYears(-15), 1),
                new Bank(3, "Deutsche Bank", DateTime.UtcNow.AddYears(-8), 1),
                new Bank(4, "JP Morgan Chase", DateTime.UtcNow.AddYears(-12), 1),
                new Bank(5, "Citibank", DateTime.UtcNow.AddYears(-10), 1)
            );
            await db.SaveChangesAsync();
        }
    }
}
