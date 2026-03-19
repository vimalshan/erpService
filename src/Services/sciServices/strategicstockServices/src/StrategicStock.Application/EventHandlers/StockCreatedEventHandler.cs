using MediatR;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Interfaces;
using StrategicStock.Domain.Events;

namespace StrategicStock.Application.EventHandlers;

public sealed class StockCreatedEventHandler(
    IRabbitMqPublisher publisher,
    ILogger<StockCreatedEventHandler> logger)
    : INotificationHandler<StockCreatedEvent>
{
    public async Task Handle(StockCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Stock {StockId} created for item {ItemId}",
            notification.StrategicStockId, notification.SciItemId);

        await publisher.PublishAsync(
            "strategic-stock", "stock.created", notification, cancellationToken);
    }
}
