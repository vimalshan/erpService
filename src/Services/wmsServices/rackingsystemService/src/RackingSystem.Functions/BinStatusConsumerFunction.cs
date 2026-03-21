using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using RackingSystem.Domain.Interfaces;

namespace RackingSystem.Functions;

/// <summary>RabbitMQ / Service Bus triggered function that processes bin-status-changed events.</summary>
public sealed class BinStatusConsumerFunction
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<BinStatusConsumerFunction> _logger;

    public BinStatusConsumerFunction(IUnitOfWork uow, ILogger<BinStatusConsumerFunction> logger)
    {
        _uow    = uow;
        _logger = logger;
    }

    /// <summary>Triggered by an Azure Service Bus queue wired from RabbitMQ shovel or direct publish.</summary>
    [Function("BinStatusConsumer")]
    public async Task Run(
        [ServiceBusTrigger("racking-bin-events", Connection = "ServiceBusConnection")] string messageBody,
        CancellationToken ct)
    {
        _logger.LogInformation("BinStatusConsumer received: {Body}", messageBody);

        var evt = JsonSerializer.Deserialize<BinStatusMessage>(messageBody);
        if (evt is null) return;

        var bin = await _uow.Bins.GetByIdAsync(evt.BinId, ct);
        if (bin is null)
        {
            _logger.LogWarning("Bin {BinId} not found; skipping.", evt.BinId);
            return;
        }

        _logger.LogInformation("Bin {BinId} status verified: current={Current}, event={EventStatus}",
            bin.Id, bin.Status, evt.NewStatus);
    }

    private sealed record BinStatusMessage(int BinId, string PreviousStatus, string NewStatus, DateTime Timestamp);
}
