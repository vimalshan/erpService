using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Todos.Functions.Triggers;

/// <summary>
/// RabbitMQ-triggered function for processing learning feedback submissions
/// </summary>
public class ProcessFeedbackSubmission
{
    private readonly ILogger _logger;

    public ProcessFeedbackSubmission(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<ProcessFeedbackSubmission>();
    }

    [Function("ProcessFeedbackSubmission")]
    public async Task Run(
        [RabbitMQTrigger("learning-feedback-queue")] FeedbackMessage message,
        FunctionContext context)
    {
        _logger.LogInformation("Processing feedback submission from RabbitMQ: {RequestNumber}", message.RequestNumber);

        try
        {
            // TODO: Implement logic to process feedback
            // This could include:
            // - Updating learning records
            // - Triggering notifications
            // - Archiving feedback data

            await Task.CompletedTask;
            _logger.LogInformation("Feedback processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing feedback: {RequestNumber}", message.RequestNumber);
            throw;
        }
    }
}

/// <summary>
/// Model for feedback messages from RabbitMQ
/// </summary>
public class FeedbackMessage
{
    public decimal RequestNumber { get; set; }
    public char FeedbackStatus { get; set; }
    public string? Comments { get; set; }
    public DateTime SubmittedAt { get; set; }
}
