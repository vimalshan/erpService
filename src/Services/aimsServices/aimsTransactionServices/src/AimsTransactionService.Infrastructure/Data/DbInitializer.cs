using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AimsTransactionService.Domain.Aggregates;

namespace AimsTransactionService.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(AimsTransactionDbContext context, ILogger logger)
    {
        if (await context.Swipes.AnyAsync())
        {
            logger.LogInformation("Database already seeded.");
            return;
        }

        logger.LogInformation("Seeding database...");

        var swipe1 = SwipeAggregate.Record(1, 100, 1, DateTime.UtcNow.AddHours(-8), 'I', 1, null, 1);
        swipe1.ClearDomainEvents();

        var swipe2 = SwipeAggregate.Record(2, 100, 1, DateTime.UtcNow, 'O', 1, null, 1);
        swipe2.ClearDomainEvents();

        await context.Swipes.AddRangeAsync(swipe1, swipe2);
        await context.SaveChangesAsync();

        logger.LogInformation("Seeding complete. 2 swipe records added.");
    }
}
