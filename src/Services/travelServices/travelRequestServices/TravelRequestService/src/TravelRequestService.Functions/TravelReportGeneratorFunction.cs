using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TravelRequestService.Functions;

public class TravelReportGeneratorFunction
{
    private readonly ILogger<TravelReportGeneratorFunction> _logger;

    public TravelReportGeneratorFunction(ILogger<TravelReportGeneratorFunction> logger)
    {
        _logger = logger;
    }

    [Function("TravelReportGenerator")]
    public async Task Run([TimerTrigger("0 0 6 * * 1")] TimerInfo timerInfo)
    {
        _logger.LogInformation("TravelReportGenerator function started at: {Time}", DateTime.UtcNow);

        // Generate weekly travel expense reports
        // In production, query the database and generate reports
        _logger.LogInformation("Weekly report generation completed.");

        await Task.CompletedTask;
    }
}
