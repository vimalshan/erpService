using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Commands.UpdateStrategicStock;

namespace StrategicStock.Infrastructure.Messaging;

public sealed record StockUpdateMessage(int StrategicStockId, long? MaxQty, long? FilledQty, string? StockTypeCode);

public sealed class StockUpdatedConsumer(
    IConfiguration configuration,
    ILogger<StockUpdatedConsumer> logger,
    IServiceProvider serviceProvider)
    : RabbitMqConsumerBase<StockUpdateMessage>(
        configuration, logger, "strategic-stock-updates", "strategic-stock", "stock.updated")
{
    protected override async Task HandleMessageAsync(StockUpdateMessage message, CancellationToken ct)
    {
        using var scope = serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        await mediator.Send(new UpdateStrategicStockCommand(
            message.StrategicStockId,
            message.MaxQty,
            message.FilledQty,
            message.StockTypeCode,
            null), ct);

        logger.LogInformation("Processed stock update for {StockId}", message.StrategicStockId);
    }
}
