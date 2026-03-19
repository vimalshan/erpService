using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using StrategicStock.Application.Queries.GetAllStrategicStocks;

namespace StrategicStock.Functions;

public sealed class StockReportFunction(IMediator mediator, ILogger<StockReportFunction> logger)
{
    /// <summary>
    /// Runs every Monday at 8 AM to generate a weekly stock report summary.
    /// </summary>
    [Function("StockReportFunction")]
    public async Task Run([TimerTrigger("0 0 8 * * MON")] TimerInfo timerInfo, CancellationToken ct)
    {
        logger.LogInformation("StockReportFunction triggered at {Time}", DateTime.UtcNow);

        var stocks = await mediator.Send(new GetAllStrategicStocksQuery(), ct);

        var activeStocks = stocks.Where(s =>
            string.IsNullOrEmpty(s.ClosureDate) ||
            (DateTime.TryParse(s.ClosureDate, out var d) && d >= DateTime.UtcNow.Date)).ToList();

        logger.LogInformation("Weekly Report: Total={Total}, Active={Active}",
            stocks.Count, activeStocks.Count);
    }
}
