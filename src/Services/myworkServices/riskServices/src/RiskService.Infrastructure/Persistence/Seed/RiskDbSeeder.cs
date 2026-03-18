using Microsoft.EntityFrameworkCore;
using RiskService.Domain.Entities;
using RiskService.Infrastructure.Persistence;

namespace RiskService.Infrastructure.Persistence.Seed;

public static class RiskDbSeeder
{
    public static async Task SeedAsync(RiskDbContext context)
    {
        if (await context.RiskTypes.AnyAsync()) return;

        // Risk Types
        context.RiskTypes.AddRange(
            new RiskType { Name = "Strategic", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskType { Name = "Operational", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskType { Name = "Financial", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskType { Name = "Compliance", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskType { Name = "Reputational", CreatedBy = 1, CreatedOn = DateTime.UtcNow }
        );

        // Risk Impacts
        context.RiskImpacts.AddRange(
            new RiskImpact { Rank = 1, Name = "Insignificant", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskImpact { Rank = 2, Name = "Minor", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskImpact { Rank = 3, Name = "Moderate", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskImpact { Rank = 4, Name = "Major", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskImpact { Rank = 5, Name = "Catastrophic", CreatedBy = 1, CreatedOn = DateTime.UtcNow }
        );

        // Risk Probabilities
        context.RiskProbabilities.AddRange(
            new RiskProbability { Rank = 1, Name = "Rare", Occurrence = "Less than 5%", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskProbability { Rank = 2, Name = "Unlikely", Occurrence = "5-25%", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskProbability { Rank = 3, Name = "Possible", Occurrence = "25-50%", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskProbability { Rank = 4, Name = "Likely", Occurrence = "50-75%", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskProbability { Rank = 5, Name = "Almost Certain", Occurrence = "More than 75%", CreatedBy = 1, CreatedOn = DateTime.UtcNow }
        );

        // Risk Ratings
        context.RiskRatings.AddRange(
            new RiskRating { Rank = 1, RatingFrom = 1, RatingTo = 4, Name = "Low", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskRating { Rank = 2, RatingFrom = 5, RatingTo = 9, Name = "Medium", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskRating { Rank = 3, RatingFrom = 10, RatingTo = 16, Name = "High", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskRating { Rank = 4, RatingFrom = 17, RatingTo = 25, Name = "Critical", CreatedBy = 1, CreatedOn = DateTime.UtcNow }
        );

        // Risk Responses
        context.RiskResponses.AddRange(
            new RiskResponse { Name = "Accept", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskResponse { Name = "Mitigate", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskResponse { Name = "Transfer", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskResponse { Name = "Avoid", CreatedBy = 1, CreatedOn = DateTime.UtcNow }
        );

        // Risk Functions
        context.RiskFunctions.AddRange(
            new RiskFunction { Name = "Finance", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskFunction { Name = "Human Resources", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskFunction { Name = "Information Technology", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskFunction { Name = "Operations", CreatedBy = 1, CreatedOn = DateTime.UtcNow },
            new RiskFunction { Name = "Legal & Compliance", CreatedBy = 1, CreatedOn = DateTime.UtcNow }
        );

        await context.SaveChangesAsync();
    }
}
