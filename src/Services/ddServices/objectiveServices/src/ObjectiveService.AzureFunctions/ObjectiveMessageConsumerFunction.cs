using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ObjectiveService.AzureFunctions;

/// <summary>
/// Queue-triggered Azure Function that processes messages placed on the
/// "objective-messages" Azure Storage Queue by the RabbitMQ bridge or direct producers.
/// </summary>
public class ObjectiveMessageConsumerFunction
{
    private readonly ILogger<ObjectiveMessageConsumerFunction> _logger;

    public ObjectiveMessageConsumerFunction(ILogger<ObjectiveMessageConsumerFunction> logger) =>
        _logger = logger;

    [Function("ObjectiveMessageConsumer")]
    public async Task Run(
        [QueueTrigger("objective-messages", Connection = "AzureWebJobsStorage")] string messageBody,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing queue message: {Message}", messageBody);

        try
        {
            var envelope = System.Text.Json.JsonSerializer.Deserialize<MessageEnvelope>(messageBody);
            if (envelope is null)
            {
                _logger.LogWarning("Could not deserialize queue message.");
                return;
            }

            _logger.LogInformation("Event type: {EventType}", envelope.EventType);

            switch (envelope.EventType)
            {
                case "GoalCreated":
                    _logger.LogInformation("Handling GoalCreated for payload: {Payload}", envelope.Payload);
                    break;

                case "ControlPointModified":
                    _logger.LogInformation("Handling ControlPointModified for payload: {Payload}", envelope.Payload);
                    break;

                default:
                    _logger.LogWarning("Unknown event type: {EventType}", envelope.EventType);
                    break;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing queue message");
            throw; // Let the Functions runtime handle retry
        }
    }
}

public class MessageEnvelope
{
    public string EventType { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public DateTime OccurredOn { get; set; }
}
