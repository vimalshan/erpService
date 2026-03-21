using ConfigService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConfigService.Infrastructure.Persistence;

public static class ConfigDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ConfigDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ConfigDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.Currencies.AnyAsync())
            {
                context.Currencies.AddRange(
                    Currency.Create(0, "INR", "Indian Rupee", "₹"),
                    Currency.Create(0, "USD", "US Dollar", "$"),
                    Currency.Create(0, "EUR", "Euro", "€"),
                    Currency.Create(0, "GBP", "British Pound", "£")
                );
                // Let DB assign identity — clear tracked IDs
                await context.SaveChangesAsync();
            }

            if (!await context.ExpenseCurrencies.AnyAsync())
            {
                context.ExpenseCurrencies.AddRange(
                    ExpenseCurrency.Create("INR", "Indian Rupee", "INR", "₹"),
                    ExpenseCurrency.Create("USD", "US Dollar", "USD", "$"),
                    ExpenseCurrency.Create("EUR", "Euro", "EUR", "€")
                );
                await context.SaveChangesAsync();
            }

            if (!await context.ExpenseTypes.AnyAsync())
            {
                context.ExpenseTypes.AddRange(
                    ExpenseType.Create(0, "Local Conveyance", 1, "DOM", 1),
                    ExpenseType.Create(0, "Food Allowance", 1, "DOM", 2),
                    ExpenseType.Create(0, "Hotel Stay", 2, "DOM", 3),
                    ExpenseType.Create(0, "Air Fare", 3, "INT", 4)
                );
                await context.SaveChangesAsync();
            }

            if (!await context.TravelClasses.AnyAsync())
            {
                context.TravelClasses.AddRange(
                    TravelClass.Create("CLS001", "AIR", "Economy", "1"),
                    TravelClass.Create("CLS002", "AIR", "Business", "2"),
                    TravelClass.Create("CLS003", "RAIL", "AC First Class", "1"),
                    TravelClass.Create("CLS004", "RAIL", "AC 2-Tier", "2")
                );
                await context.SaveChangesAsync();
            }

            if (!await context.GlobalPayParams.AnyAsync())
            {
                context.GlobalPayParams.AddRange(
                    GlobalPayParam.Create("P001", "MAX_ADVANCE", "Maximum Advance Amount", "50000"),
                    GlobalPayParam.Create("P002", "SETTLEMENT_DAYS", "Settlement Days", "30"),
                    GlobalPayParam.Create("P003", "AUTO_APPROVE_LIMIT", "Auto Approve Limit", "5000")
                );
                await context.SaveChangesAsync();
            }

            logger.LogInformation("Seed data applied successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database.");
        }
    }
}
