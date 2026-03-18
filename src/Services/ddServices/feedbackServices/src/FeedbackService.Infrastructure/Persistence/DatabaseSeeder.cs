namespace FeedbackService.Infrastructure.Persistence;

using Domain.Aggregates;

/// <summary>
/// Database seeding service
/// </summary>
public static class DatabaseSeeder
{
    /// <summary>
    /// Seeds initial data into the database
    /// </summary>
    public static async Task SeedAsync(FeedbackDbContext context)
    {
        try
        {
            // Check if data already exists
            if (context.Feedbacks.Any())
            {
                return;
            }

            // Seed feedback data
            var feedbackList = new List<Feedback>
            {
                Feedback.Create(
                    feedbackId: 1,
                    requestNo: 100,
                    approverSystemId: 5),

                Feedback.Create(
                    feedbackId: 2,
                    requestNo: 101,
                    approverSystemId: 6),

                Feedback.Create(
                    feedbackId: 3,
                    requestNo: 102,
                    approverSystemId: 7)
            };

            // Add items to feedbacks
            if (feedbackList.Count > 0)
            {
                feedbackList[0].UpdateRemarks("Initial feedback for request 100");
                feedbackList[0].AddItem(1, 101);
                feedbackList[0].AddItem(2, 102);

                feedbackList[1].UpdateRemarks("Initial feedback for request 101");
                feedbackList[1].AddItem(1, 103);
                feedbackList[1].AddItem(2, null);

                feedbackList[2].UpdateRemarks("Initial feedback for request 102");
                feedbackList[2].AddItem(1, 104);
            }

            await context.Feedbacks.AddRangeAsync(feedbackList);
            await context.SaveChangesAsync();

            System.Diagnostics.Debug.WriteLine($"Database seeded with {feedbackList.Count} feedback records");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error seeding database: {ex.Message}");
            throw;
        }
    }
}
