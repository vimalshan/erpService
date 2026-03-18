using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace InventoryManagement.Functions.Functions;

/// <summary>
/// HTTP-triggered Azure Function for processing inventory events forwarded by RabbitMQ bridge.
/// </summary>
public sealed class ProcessInventoryMessageFunction
{
    private readonly ILogger<ProcessInventoryMessageFunction> _logger;

    public ProcessInventoryMessageFunction(ILogger<ProcessInventoryMessageFunction> logger)
        => _logger = logger;

    [Function(nameof(ProcessInventoryMessageFunction))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "inventory/process")] HttpRequest req)
    {
        _logger.LogInformation("ProcessInventoryMessageFunction triggered.");

        using var reader = new StreamReader(req.Body);
        var body = await reader.ReadToEndAsync();
        _logger.LogInformation("Received payload: {Body}", body);

        try
        {
            var message = JsonSerializer.Deserialize<Dictionary<string, object>>(body);
            _logger.LogInformation("Processing event type: {EventType}",
                message?.GetValueOrDefault("eventType"));

            // Process the event — update analytics, trigger notifications, etc.

            return new OkObjectResult("Processed successfully.");
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Invalid JSON payload received.");
            return new BadRequestObjectResult("Invalid JSON payload.");
        }
    }
}
