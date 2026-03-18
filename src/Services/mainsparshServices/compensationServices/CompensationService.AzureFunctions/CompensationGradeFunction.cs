using CompensationService.Application.Commands;
using CompensationService.Application.Queries;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using MediatR;

namespace CompensationService.AzureFunctions;

/// <summary>
/// Azure Functions for Compensation Service background tasks
/// </summary>
public class CompensationGradeFunction
{
    private readonly IMediator _mediator;
    private readonly ILogger<CompensationGradeFunction> _logger;

    public CompensationGradeFunction(IMediator mediator, ILogger<CompensationGradeFunction> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Timer-triggered function to process compensation grade updates
    /// Runs every day at 2 AM UTC
    /// </summary>
    [Function("ProcessCompensationGradeUpdates")]
    public async Task ProcessGradeUpdatesAsync([TimerTrigger("0 0 2 * * *")] TimerInfo myTimer)
    {
        try
        {
            _logger.LogInformation($"Processing compensation grade updates at {DateTime.UtcNow}");

            // Get all active grades
            var query = new GetActiveCompensationGradesQuery();
            var activeGrades = await _mediator.Send(query);

            _logger.LogInformation($"Found {activeGrades.Count()} active grades");
            
            // Process each grade (e.g., check if effective dates have changed)
            foreach (var grade in activeGrades)
            {
                _logger.LogDebug($"Processing grade: {grade.GradeCode}");
            }

            _logger.LogInformation("Compensation grade updates processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing compensation grade updates: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// RabbitMQ-triggered function to handle compensation grade events
    /// </summary>
    [Function("ProcessCompensationGradeEvents")]
    public async Task ProcessGradeEventsAsync(
        [RabbitMQTrigger("compensation-grade-events")] string message)
    {
        try
        {
            _logger.LogInformation($"Processing compensation grade event: {message}");
            
            // Process the event (could be deserialized and handled based on type)
            _logger.LogInformation("Compensation grade event processed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing compensation grade event: {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Blob storage-triggered function to handle image uploads
    /// </summary>
    [Function("ProcessStationeryImageUpload")]
    public async Task ProcessImageUploadAsync(
        [BlobTrigger("stationery-images/{name}", Connection = "AzureWebJobsStorage")] Stream image,
        string name)
    {
        try
        {
            _logger.LogInformation($"Processing image upload: {name}, Size: {image.Length} bytes");
            
            // Process image (validation, thumbnail creation, etc.)
            _logger.LogInformation($"Image {name} processed successfully");

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing image upload: {ex.Message}");
            throw;
        }
    }
}
