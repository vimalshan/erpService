using EmployeeManagement.Application.Common.Interfaces;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.Functions.Functions;

/// <summary>
/// HTTP-triggered Azure Function that processes inbound employee domain events
/// (acts as a webhook receiver, e.g. from a Service Bus / API Gateway).
/// </summary>
public class EmployeeEventFunction(IMessagePublisher messagePublisher,
    ILogger<EmployeeEventFunction> logger)
{
    [Function(nameof(EmployeeEventFunction))]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "events/employee")] HttpRequestData req)
    {
        logger.LogInformation("EmployeeEventFunction received a request");

        string body;
        using (var reader = new StreamReader(req.Body))
            body = await reader.ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(body))
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Request body is empty.");
            return bad;
        }

        EmployeeEventPayload? payload;
        try
        {
            payload = JsonSerializer.Deserialize<EmployeeEventPayload>(body,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (JsonException ex)
        {
            logger.LogWarning(ex, "Invalid JSON payload");
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("Invalid JSON payload.");
            return bad;
        }

        if (payload is null || string.IsNullOrWhiteSpace(payload.EventType))
        {
            var bad = req.CreateResponse(HttpStatusCode.BadRequest);
            await bad.WriteStringAsync("EventType is required.");
            return bad;
        }

        var routingKey = payload.EventType.ToLowerInvariant() switch
        {
            "employeecreated"    => "hr.employee.created",
            "employeepromoted"   => "hr.employee.promoted",
            "employeetransferred"=> "hr.employee.transferred",
            _                    => "hr.employee.events"
        };

        await messagePublisher.PublishAsync("hr.exchange", routingKey, payload);

        logger.LogInformation("Event '{EventType}' forwarded to '{RoutingKey}'",
            payload.EventType, routingKey);

        var ok = req.CreateResponse(HttpStatusCode.Accepted);
        await ok.WriteStringAsync($"Event '{payload.EventType}' accepted.");
        return ok;
    }

    private sealed record EmployeeEventPayload(string EventType, int EmployeeId, object? Data);
}
