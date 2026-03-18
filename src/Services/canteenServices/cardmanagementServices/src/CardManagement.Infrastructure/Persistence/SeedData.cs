using CardManagement.Domain.Entities;
using CardManagement.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CardManagement.Infrastructure.Persistence;

public static class SeedData
{
    public static async Task InitialiseAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ApplicationDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.GuestCardMasters.AnyAsync())
            {
                var cards = new[]
                {
                    GuestCardMaster.Create(1001, 1, "CARD-001", "John Guest", "G", "HQ", 10, DateTime.UtcNow.AddDays(-30), 1),
                    GuestCardMaster.Create(1002, 2, "CARD-002", "Jane Visitor", "V", "BR1", 20, DateTime.UtcNow.AddDays(-15), 1),
                    GuestCardMaster.Create(1003, 3, "CARD-003", "Temp Worker", "T", "WH", 30, DateTime.UtcNow.AddDays(-7), 1)
                };

                foreach (var card in cards)
                {
                    card.ClearDomainEvents();
                    await context.GuestCardMasters.AddAsync(card);
                }

                await context.SaveChangesAsync();
                logger.LogInformation("Seed data: {Count} guest cards created.", cards.Length);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error during database initialisation.");
            throw;
        }
    }
}
