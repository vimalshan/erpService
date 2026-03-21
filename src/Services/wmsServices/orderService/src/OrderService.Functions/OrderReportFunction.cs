using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using OrderService.Application.Interfaces;

namespace OrderService.Functions;

public class OrderReportFunction
{
    private readonly IBlobStorageService _blobStorage;
    private readonly ILogger<OrderReportFunction> _logger;

    public OrderReportFunction(IBlobStorageService blobStorage, ILogger<OrderReportFunction> logger)
    {
        _blobStorage = blobStorage;
        _logger = logger;
    }

    [Function("GenerateOrderReport")]
    public async Task Run([TimerTrigger("0 0 6 * * 1")] TimerInfo timerInfo, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Generating weekly order report at {Time}", DateTime.UtcNow);

        // Example: Generate a report and upload to blob storage
        var reportContent = $"Order Report generated at {DateTime.UtcNow:O}";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(reportContent));

        var blobName = $"reports/order-report-{DateTime.UtcNow:yyyyMMdd}.txt";
        var url = await _blobStorage.UploadAsync("order-reports", blobName, stream, "text/plain", cancellationToken);

        _logger.LogInformation("Order report uploaded to {Url}", url);
    }
}
