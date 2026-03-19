using MediatR;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Interfaces;
using StrategicStock.Domain.Events;

namespace StrategicStock.Application.EventHandlers;

public sealed class StockUpdatedEventHandler(
    IRabbitMqPublisher publisher,
    ILogger<StockUpdatedEventHandler> logger)
    : INotificationHandler<StockUpdatedEvent>
{
    public async Task Handle(StockUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Stock {StockId} updated", notification.StrategicStockId);

        await publisher.PublishAsync(
            "strategic-stock", "stock.updated.event", notification, cancellationToken);
    }
}
