using InvestmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InvestmentService.Infrastructure.Data.Seed;

public static class InvestmentDbSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InvestmentDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<InvestmentDbContext>>();

        try
        {
            await db.Database.MigrateAsync();

            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    new InvestmentCategory { Code = 1, ShortCode = "GOV", Name = "Government Securities", Denomination = 100, GroupId = 1 },
                    new InvestmentCategory { Code = 2, ShortCode = "CORP", Name = "Corporate Bonds", Denomination = 1000, GroupId = 1 },
                    new InvestmentCategory { Code = 3, ShortCode = "FD", Name = "Fixed Deposits", Denomination = 1000, GroupId = 2 },
                    new InvestmentCategory { Code = 4, ShortCode = "MF", Name = "Mutual Funds", Denomination = 10, GroupId = 3 },
                    new InvestmentCategory { Code = 5, ShortCode = "TD", Name = "Term Deposits", Denomination = 1000, GroupId = 2 },
                    new InvestmentCategory { Code = 6, ShortCode = "SLR", Name = "SLR Securities", Denomination = 100, GroupId = 1 }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.SubCategories.AnyAsync())
            {
                db.SubCategories.AddRange(
                    new InvestmentSubCategory { Id = 1, ShortName = "GSEC", Name = "G-Sec Bonds", CategoryId = 1 },
                    new InvestmentSubCategory { Id = 2, ShortName = "SDL", Name = "State Development Loans", CategoryId = 1 },
                    new InvestmentSubCategory { Id = 3, ShortName = "TBILL", Name = "Treasury Bills", CategoryId = 1 },
                    new InvestmentSubCategory { Id = 4, ShortName = "NCD", Name = "Non-Convertible Debentures", CategoryId = 2 },
                    new InvestmentSubCategory { Id = 5, ShortName = "CP", Name = "Commercial Paper", CategoryId = 2 },
                    new InvestmentSubCategory { Id = 6, ShortName = "PSU", Name = "PSU Bonds", CategoryId = 2 },
                    new InvestmentSubCategory { Id = 7, ShortName = "BANKFD", Name = "Bank Fixed Deposit", CategoryId = 3 },
                    new InvestmentSubCategory { Id = 8, ShortName = "DEBT", Name = "Debt MF", CategoryId = 4 },
                    new InvestmentSubCategory { Id = 9, ShortName = "EQUITY", Name = "Equity MF", CategoryId = 4 }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.CreditAgencies.AnyAsync())
            {
                db.CreditAgencies.AddRange(
                    new CreditAgency { AgencyId = 1, AgencyName = "CRISIL" },
                    new CreditAgency { AgencyId = 2, AgencyName = "ICRA" },
                    new CreditAgency { AgencyId = 3, AgencyName = "CARE" },
                    new CreditAgency { AgencyId = 4, AgencyName = "India Ratings" },
                    new CreditAgency { AgencyId = 5, AgencyName = "Brickwork Ratings" }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.CreditRatings.AnyAsync())
            {
                db.CreditRatings.AddRange(
                    new CreditRating { RatingId = 1, RatingName = "AAA" },
                    new CreditRating { RatingId = 2, RatingName = "AA+" },
                    new CreditRating { RatingId = 3, RatingName = "AA" },
                    new CreditRating { RatingId = 4, RatingName = "AA-" },
                    new CreditRating { RatingId = 5, RatingName = "A+" },
                    new CreditRating { RatingId = 6, RatingName = "A" },
                    new CreditRating { RatingId = 7, RatingName = "BBB" }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.Brokers.AnyAsync())
            {
                db.Brokers.AddRange(
                    new Broker { BrokerId = 1, BrokerName = "SBI DFHI", BrokerStatus = "A" },
                    new Broker { BrokerId = 2, BrokerName = "ICICI Securities", BrokerStatus = "A" },
                    new Broker { BrokerId = 3, BrokerName = "HDFC Securities", BrokerStatus = "A" },
                    new Broker { BrokerId = 4, BrokerName = "Kotak Securities", BrokerStatus = "A" }
                );
                await db.SaveChangesAsync();
            }

            logger.LogInformation("Investment database seeded successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding investment database");
            throw;
        }
    }
}
