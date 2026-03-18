using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ReviewService.Domain.Entities;
using ReviewService.Infrastructure.Data;

namespace ReviewService.Infrastructure.Data.Seeds;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ReviewDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<ReviewDbContext>>();

        try
        {
            await context.Database.MigrateAsync();

            if (!await context.ReviewMasts.AnyAsync())
            {
                context.ReviewMasts.AddRange(
                    ReviewMast.Create("ANN", "Annual Review", "PERFORMANCE"),
                    ReviewMast.Create("MID", "Mid-Year Review", "PERFORMANCE"),
                    ReviewMast.Create("TRN", "Training Review", "LEARNING"),
                    ReviewMast.Create("CRS", "Course Review", "LEARNING"),
                    ReviewMast.Create("SKL", "Skill Review", "COMPETENCY")
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded REVIEW_MAST data.");
            }

            if (!await context.FeedMasts.AnyAsync())
            {
                context.FeedMasts.AddRange(
                    FeedMast.Create(1, "Overall Satisfaction", 'N', "OVERALL"),
                    FeedMast.Create(2, "Content Quality", 'N', "CONTENT"),
                    FeedMast.Create(3, "Trainer Effectiveness", 'N', "TRAINER"),
                    FeedMast.Create(4, "Facilities", 'N', "FACILITY"),
                    FeedMast.Create(5, "Would Recommend", 'Y', "RECOMMEND")
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded FEED_MAST data.");
            }

            if (!await context.FeedEvalMasts.AnyAsync())
            {
                context.FeedEvalMasts.AddRange(
                    FeedEvalMast.Create(1, "Excellent (9-10)", 10),
                    FeedEvalMast.Create(2, "Good (7-8)", 8),
                    FeedEvalMast.Create(3, "Average (5-6)", 6),
                    FeedEvalMast.Create(4, "Below Average (3-4)", 4),
                    FeedEvalMast.Create(5, "Poor (1-2)", 2)
                );
                await context.SaveChangesAsync();
                logger.LogInformation("Seeded FEED_EVALMAST data.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database.");
            throw;
        }
    }
}
