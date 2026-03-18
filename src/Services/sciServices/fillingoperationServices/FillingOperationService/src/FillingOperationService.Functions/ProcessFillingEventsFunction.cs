using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text;
using System.Text.Json;

namespace FillingOperationService.Functions;

/// <summary>
/// Processes messages from the RabbitMQ-backed filling-operations-events queue
/// via an HTTP-triggered function (webhook pattern for Azure Functions + RabbitMQ).
/// </summary>
public class ProcessFillingEventsFunction(ILogger<ProcessFillingEventsFunction> logger)
{
    [Function("ProcessFillingEvent")]
    public async Task<HttpResponseData> RunAsync(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "events/filling")] HttpRequestData req,
        CancellationToken cancellationToken)
    {
        var body = await new StreamReader(req.Body, Encoding.UTF8).ReadToEndAsync(cancellationToken);

        logger.LogInformation("Received filling event payload: {Payload}", body);

        try
        {
            using var doc = JsonDocument.Parse(body);
            var eventType = doc.RootElement.TryGetProperty("eventType", out var et) ? et.GetString() : "unknown";

            logger.LogInformation("Processing event type: {EventType}", eventType);

            // Additional event processing logic (e.g., update caches, send notifications)
            // would be dispatched here via MediatR or direct service calls.
        }
        catch (JsonException jex)
        {
            logger.LogWarning(jex, "Invalid JSON payload received.");
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Invalid JSON.", cancellationToken);
            return badResponse;
        }

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("Event processed.", cancellationToken);
        return response;
    }
}
