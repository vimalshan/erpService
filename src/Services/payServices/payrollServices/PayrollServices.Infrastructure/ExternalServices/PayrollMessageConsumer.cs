using PayrollServices.Domain.Entities;
using PayrollServices.Infrastructure.Messaging;
using RabbitMQ.Client;
using System.Text.Json;

namespace PayrollServices.Infrastructure.ExternalServices;

/// <summary>
/// Payload consumer for processing payroll-related messages
/// </summary>
public class PayrollMessageConsumer : RabbitMqConsumerBase
{
    public PayrollMessageConsumer(IConnection connection)
        : base(connection, "payroll.queue", "payroll.exchange", "payroll.*")
    {
    }

    public override async Task ProcessMessageAsync(string message)
    {
        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var payloadData = JsonSerializer.Deserialize<Dictionary<string, object>>(message, options);

            if (payloadData != null && payloadData.TryGetValue("type", out var messageType))
            {
                var type = messageType?.ToString();
                switch (type)
                {
                    case "PayrollProcessed":
                        await HandlePayrollProcessedAsync(payloadData);
                        break;
                    case "AdjustmentCreated":
                        await HandleAdjustmentCreatedAsync(payloadData);
                        break;
                    default:
                        Console.WriteLine($"Unknown message type: {type}");
                        break;
                }
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing message: {ex.Message}");
        }
    }

    private async Task HandlePayrollProcessedAsync(Dictionary<string, object> payload)
    {
        // Implementation for handling payroll processed event
        await Task.CompletedTask;
    }

    private async Task HandleAdjustmentCreatedAsync(Dictionary<string, object> payload)
    {
        // Implementation for handling adjustment created event
        await Task.CompletedTask;
    }
}
