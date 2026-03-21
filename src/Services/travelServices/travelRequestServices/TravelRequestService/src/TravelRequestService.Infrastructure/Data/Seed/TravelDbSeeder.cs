using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TravelRequestService.Domain.Entities;
using TravelRequestService.Domain.Enums;

namespace TravelRequestService.Infrastructure.Data.Seed;

public static class TravelDbSeeder
{
    public static async Task SeedAsync(TravelDbContext context, ILogger logger)
    {
        try
        {
            if (!await context.TravelMains.AnyAsync())
            {
                var travels = new List<TravelMain>
                {
                    TravelMain.Create("001", 1, 1001, "Client meeting in Mumbai", TravelType.Domestic, 25000),
                    TravelMain.Create("001", 2, 1002, "Conference in Singapore", TravelType.International, 150000),
                    TravelMain.Create("001", 3, 1001, "Training in Bangalore", TravelType.Domestic, 15000),
                };

                // Clear domain events from seed data
                foreach (var travel in travels)
                    travel.ClearDomainEvents();

                await context.TravelMains.AddRangeAsync(travels);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} travel requests", travels.Count);
            }

            if (!await context.TravelAgendas.AnyAsync())
            {
                var agendas = new[]
                {
                    TravelAgenda.Create(1, 1, DateTime.UtcNow.AddDays(5), "Mr. Sharma", "Discuss project timeline", "Mumbai"),
                    TravelAgenda.Create(1, 2, DateTime.UtcNow.AddDays(6), "Mrs. Patel", "Budget review", "Mumbai"),
                    TravelAgenda.Create(2, 1, DateTime.UtcNow.AddDays(10), "Tech Team", "Architecture workshop", "Singapore"),
                };

                await context.TravelAgendas.AddRangeAsync(agendas);
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded {Count} travel agendas", agendas.Length);
            }

            logger.LogInformation("Database seeding completed");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }
}
