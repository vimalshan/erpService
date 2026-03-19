using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Commands.CloseStrategicStock;
using StrategicStock.Application.Queries.GetAllStrategicStocks;

namespace StrategicStock.Functions;

public sealed class StockCleanupFunction(IMediator mediator, ILogger<StockCleanupFunction> logger)
{
    /// <summary>
    /// Runs daily at midnight to identify and close expired strategic stocks.
    /// </summary>
    [Function("StockCleanupFunction")]
    public async Task Run([TimerTrigger("0 0 0 * * *")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("StockCleanupFunction triggered at {Time}", DateTime.UtcNow);

        var stocks = await mediator.Send(new GetAllStrategicStocksQuery(), ct);
        var expiredCount = 0;

        foreach (var stock in stocks)
        {
            if (!string.IsNullOrEmpty(stock.ClosureDate) &&
                DateTime.TryParse(stock.ClosureDate, out var closureDate) &&
                closureDate < DateTime.UtcNow.Date)
            {
                expiredCount++;
                logger.LogInformation("Closing expired stock {Id} (closure: {Date})",
                    stock.StrategicStockId, stock.ClosureDate);
                await mediator.Send(new CloseStrategicStockCommand(stock.StrategicStockId, null), ct);
            }
        }

        logger.LogInformation("StockCleanupFunction completed. Closed {Count} expired stocks.", expiredCount);
    }
}
