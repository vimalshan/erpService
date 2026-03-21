using ExpenseService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ExpenseService.Infrastructure.Data;

public static class SeedData
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ExpenseDbContext>>();

        try
        {
            await context.Database.MigrateAsync();
            logger.LogInformation("Database migrated successfully.");

            if (!await context.DaRules.AnyAsync())
            {
                context.DaRules.AddRange(
                    new DaRule
                    {
                        SerialNumber = 1, BandId = 1, CountryCode = 1,
                        SelfBookingFlag = "N", CurrencyCode = "INR",
                        BudgetAmount = 1500, EffectiveDate = new DateTime(2024, 1, 1)
                    },
                    new DaRule
                    {
                        SerialNumber = 2, BandId = 2, CountryCode = 1,
                        SelfBookingFlag = "N", CurrencyCode = "INR",
                        BudgetAmount = 2000, EffectiveDate = new DateTime(2024, 1, 1)
                    },
                    new DaRule
                    {
                        SerialNumber = 3, BandId = 1, CountryCode = 2,
                        SelfBookingFlag = "Y", CurrencyCode = "USD",
                        BudgetAmount = 100, EffectiveDate = new DateTime(2024, 1, 1)
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("DA Rules seed data added.");
            }

            if (!await context.TravelExpenses.AnyAsync())
            {
                context.TravelExpenses.AddRange(
                    new TravelExpense
                    {
                        RequestNumber = 1001, SerialNumber = 1,
                        ExpenseCode = 10, CurrencyType = "INR",
                        EligibleAmount = 5000, BudgetAmount = 4500,
                        SelfExpense = 1000, VarianceAmount = 500,
                        ExpenseRemarks = "Travel to client site"
                    },
                    new TravelExpense
                    {
                        RequestNumber = 1001, SerialNumber = 2,
                        ExpenseCode = 20, CurrencyType = "INR",
                        EligibleAmount = 3000, BudgetAmount = 3000,
                        SelfExpense = 500, VarianceAmount = 0,
                        ExpenseRemarks = "Hotel stay"
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Travel Expense seed data added.");
            }

            if (!await context.TravelConveyances.AnyAsync())
            {
                context.TravelConveyances.AddRange(
                    new TravelConveyance
                    {
                        SerialNumber = 1, RequestNumber = 1001,
                        Date = new DateTime(2024, 6, 15),
                        Particulars = "Cab from airport to hotel",
                        Mode = 1, Amount = 800
                    }
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Conveyance seed data added.");
            }

            if (!await context.DaSummaries.AnyAsync())
            {
                context.DaSummaries.Add(new DaSummary
                {
                    RequestId = 1001,
                    AdminHours = 72, AdminDays = 3, AdminRate = 1500, AdminAmount = 4500,
                    SelfHours = 0, SelfDays = 0, SelfRate = 0, SelfAmount = 0
                });
                await context.SaveChangesAsync();
                logger.LogInformation("DA Summary seed data added.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while seeding the database.");
        }
    }
}
