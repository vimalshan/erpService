using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MedicalVisit.Functions.Functions;

public class VisitReminderFunction
{
    private readonly ILogger<VisitReminderFunction> _logger;

    public VisitReminderFunction(ILogger<VisitReminderFunction> logger)
    {
        _logger = logger;
    }

    // Runs every day at 8 AM UTC
    [Function("VisitReminder")]
    public async Task RunAsync([TimerTrigger("0 0 8 * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Visit Reminder function triggered at: {Time}", DateTime.UtcNow);

        try
        {
            // Find visits scheduled for follow-up today
            // TODO: Inject IVisitRepository and IMediator through constructor
            // and query for upcoming revisits

            _logger.LogInformation("Processing follow-up visit reminders for {Date}", DateOnly.FromDateTime(DateTime.UtcNow));

            // Integration point: Send notifications via email/SMS for visits with NextReviewDate = today
            await Task.CompletedTask;

            _logger.LogInformation("Visit reminder notifications dispatched successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing visit reminders");
            throw;
        }
    }
}
