using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OrderService.Domain.Aggregates;

namespace OrderService.Infrastructure.Persistence;

public static class OrderDbContextSeed
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.Orders.AnyAsync())
            {
                var order1 = Order.Create(1, "system", DateTime.UtcNow.AddDays(7));
                order1.AddItem(productId: 1, quantity: 5, unitPrice: 12.99m, discount: 0, notes: "Notebooks");
                order1.AddItem(productId: 2, quantity: 10, unitPrice: 1.50m, discount: 2.00m, notes: "Pens");

                var order2 = Order.Create(2, "system", DateTime.UtcNow.AddDays(14));
                order2.AddItem(productId: 3, quantity: 2, unitPrice: 250.00m, discount: 0);

                context.Orders.AddRange(order1, order2);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} orders into the database.", 2);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
