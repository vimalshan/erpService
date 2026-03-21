using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ShipmentService.Application.Common.Interfaces;
using ShipmentService.Application.Features.Shipments.Queries.GetAllShipments;
using MediatR;
using ShipmentService.Domain.Enums;

namespace ShipmentService.Functions;

/// <summary>Timer-triggered function that checks for stale IN_TRANSIT shipments and publishes alerts.</summary>
public sealed class ShipmentStalenessCheckFunction
{
    private readonly ILogger<ShipmentStalenessCheckFunction> _logger;
    private readonly IMediator _mediator;
    private readonly IMessagePublisher _publisher;

    public ShipmentStalenessCheckFunction(
        ILogger<ShipmentStalenessCheckFunction> logger,
        IMediator mediator,
        IMessagePublisher publisher)
    {
        _logger = logger;
        _mediator = mediator;
        _publisher = publisher;
    }

    // Runs every 6 hours
    [Function(nameof(ShipmentStalenessCheckFunction))]
    public async Task Run([TimerTrigger("0 0 */6 * * *")] TimerInfo timer, CancellationToken ct)
    {
        _logger.LogInformation("ShipmentStalenessCheck triggered at {Time}", DateTime.UtcNow);

        var allShipments = await _mediator.Send(new GetAllShipmentsQuery(1, 500), ct);
        var staleThreshold = DateTime.UtcNow.AddDays(-3);

        var staleShipments = allShipments.Items
            .Where(s => s.Status == ShipmentStatus.InTransit.ToString() && s.ShippedDate < staleThreshold)
            .ToList();

        if (staleShipments.Count == 0)
        {
            _logger.LogInformation("No stale shipments found.");
            return;
        }

        _logger.LogWarning("Found {Count} stale IN_TRANSIT shipments", staleShipments.Count);

        foreach (var shipment in staleShipments)
        {
            await _publisher.PublishAsync(
                "shipment.exchange",
                "shipment.stale_alert",
                new { shipment.ShipmentNumber, shipment.ShippedDate, shipment.CustomerId },
                ct);
        }
    }
}
