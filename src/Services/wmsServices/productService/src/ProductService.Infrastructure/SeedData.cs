using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductService.Domain.Entities;
using ProductService.Infrastructure.Persistence;

namespace ProductService.Infrastructure;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ProductDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.Categories.AnyAsync())
            {
                var categories = new[]
                {
                    new Category("Electronics", "Electronic devices and components"),
                    new Category("Office Supplies", "General office supplies"),
                    new Category("Raw Materials", "Manufacturing raw materials"),
                    new Category("Packaging", "Packaging materials")
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} categories", categories.Length);
            }

            if (!await context.Products.AnyAsync())
            {
                var electronics = await context.Categories.FirstAsync(c => c.CategoryName == "Electronics");
                var office = await context.Categories.FirstAsync(c => c.CategoryName == "Office Supplies");

                var products = new[]
                {
                    new Product("SKU-ELEC-001", "Wireless Mouse", "Ergonomic wireless mouse", electronics.CategoryId,
                        "EA", 0.15m, 0.001m, 29.99m, 50m, 100m),
                    new Product("SKU-ELEC-002", "USB-C Hub", "7-port USB-C hub", electronics.CategoryId,
                        "EA", 0.25m, 0.002m, 49.99m, 30m, 50m),
                    new Product("SKU-OFF-001", "A4 Paper Ream", "80gsm A4 paper, 500 sheets", office.CategoryId,
                        "PK", 2.5m, 0.003m, 8.99m, 100m, 200m),
                    new Product("SKU-OFF-002", "Ballpoint Pen Box", "Box of 50 blue ballpoint pens", office.CategoryId,
                        "BX", 0.5m, 0.001m, 12.50m, 25m, 50m)
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();

                logger.LogInformation("Seeded {Count} products", products.Length);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database");
        }
    }
}
