using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Stationery.Domain.Interfaces;
using Stationery.Application.Features.Items.Queries;
using MediatR;

namespace Stationery.Functions;

public class StockAlertFunction
{
    private readonly ILogger _logger;
    private readonly IMediator _mediator;

    public StockAlertFunction(ILoggerFactory loggerFactory, IMediator mediator)
    {
        _logger = loggerFactory.CreateLogger<StockAlertFunction>();
        _mediator = mediator;
    }

    [Function("LowStockAlert")]
    public async Task Run([TimerTrigger("0 0 9 * * *")] TimerInfo myTimer)
    {
        _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        
        var lowStockItems = await _mediator.Send(new GetLowStockItemsQuery(10));
        
        foreach (var item in lowStockItems)
        {
            _logger.LogWarning($"LOW STOCK ALERT: {item.Description} (ID: {item.Id}) has only {item.Stock} left. Reorder level: {item.ReorderLevel}");
        }
    }
}
public class TimerInfo
{
    public MyScheduleStatus? ScheduleStatus { get; set; }
    public bool IsPastDue { get; set; }
}
public class MyScheduleStatus
{
    public DateTime Last { get; set; }
    public DateTime Next { get; set; }
    public DateTime LastUpdated { get; set; }
}
