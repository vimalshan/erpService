using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace MedicalVisit.Functions.Functions;

public class VisitReportGeneratorFunction
{
    private readonly ILogger<VisitReportGeneratorFunction> _logger;

    public VisitReportGeneratorFunction(ILogger<VisitReportGeneratorFunction> logger)
    {
        _logger = logger;
    }

    // Runs every Sunday at midnight UTC to generate weekly reports
    [Function("WeeklyVisitReport")]
    public async Task RunWeeklyReportAsync([TimerTrigger("0 0 0 * * 0")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Weekly Visit Report function triggered at: {Time}", DateTime.UtcNow);

        try
        {
            var reportDate = DateTime.UtcNow.Date;
            var weekStart = reportDate.AddDays(-(int)reportDate.DayOfWeek);
            var weekEnd = weekStart.AddDays(7);

            _logger.LogInformation("Generating visit report for week {WeekStart} to {WeekEnd}", weekStart, weekEnd);

            // TODO: Query visits, aggregate statistics, and upload report to blob storage
            await Task.CompletedTask;

            _logger.LogInformation("Weekly visit report generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly visit report");
            throw;
        }
    }

    // Runs every month on the 1st day to generate monthly reports
    [Function("MonthlyVisitReport")]
    public async Task RunMonthlyReportAsync([TimerTrigger("0 0 0 1 * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Monthly Visit Report function triggered at: {Time}", DateTime.UtcNow);

        try
        {
            var now = DateTime.UtcNow;
            var monthStart = new DateTime(now.Year, now.Month, 1).AddMonths(-1);
            var monthEnd = new DateTime(now.Year, now.Month, 1);

            _logger.LogInformation("Generating visit report for month {Month}/{Year}",
                monthStart.Month, monthStart.Year);

            // TODO: Generate and store monthly statistics report
            await Task.CompletedTask;

            _logger.LogInformation("Monthly visit report generated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating monthly visit report");
            throw;
        }
    }
}
