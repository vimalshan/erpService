using MediatR;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Interfaces;
using StrategicStock.Domain.Events;

namespace StrategicStock.Application.EventHandlers;

public sealed class StockClosedEventHandler(
    IRabbitMqPublisher publisher,
    ILogger<StockClosedEventHandler> logger)
    : INotificationHandler<StockClosedEvent>
{
    public async Task Handle(StockClosedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Stock {StockId} closed on {Date}",
            notification.StrategicStockId, notification.ClosureDate);

        await publisher.PublishAsync(
            "strategic-stock", "stock.closed", notification, cancellationToken);
    }
}
