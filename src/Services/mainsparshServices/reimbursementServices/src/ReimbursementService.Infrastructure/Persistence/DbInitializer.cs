using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReimbursementService.Domain.Entities;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Domain.ValueObjects;

namespace ReimbursementService.Infrastructure.Persistence;

public static class DbInitializer
{
    public static async Task InitialiseAsync(IHost host)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migration applied successfully.");

            if (!await context.ReimTran.AnyAsync())
            {
                await SeedAsync(context);
                logger.LogInformation("Database seeded with sample data.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        var entries = new[]
        {
            ReimbursementTransaction.Create(
                "REIM-20260301-SEED0001",
                1001, ReimbursementType.Travel,
                new Money(4500.00m, "INR"),
                new DateOnly(2026, 3, 1),
                new DateOnly(2026, 2, 28),
                "Travel to client site in Mumbai",
                "Mumbai", 1001),

            ReimbursementTransaction.Create(
                "REIM-20260301-SEED0002",
                1002, ReimbursementType.Meal,
                new Money(850.00m, "INR"),
                new DateOnly(2026, 3, 2),
                new DateOnly(2026, 3, 2),
                "Team lunch during project kickoff",
                "Pune", 1002),

            ReimbursementTransaction.Create(
                "REIM-20260301-SEED0003",
                1001, ReimbursementType.Accommodation,
                new Money(3200.00m, "INR"),
                new DateOnly(2026, 3, 3),
                new DateOnly(2026, 3, 3),
                "Hotel stay - client visit",
                "Mumbai", 1001),
        };

        // Advance the first two through the workflow for richer seed data
        entries[0].Submit();
        entries[0].Approve(99, 1);

        entries[1].Submit();

        await context.ReimTran.AddRangeAsync(entries);
        await context.SaveChangesAsync();
    }
}
