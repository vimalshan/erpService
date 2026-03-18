using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Functions.BackgroundTasks;

public class SettlementReportWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SettlementReportWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public SettlementReportWorker(IServiceProvider serviceProvider, ILogger<SettlementReportWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Settlement Report Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var settlements = await unitOfWork.Settlements.GetAllAsync(stoppingToken);
                var summary = settlements.GroupBy(s => s.StStatus)
                    .Select(g => new { Status = g.Key, Count = g.Count(), TotalAmount = g.Sum(s => s.StSettlementAmount ?? 0) });

                foreach (var item in summary)
                {
                    _logger.LogInformation("Daily Report - Status: {Status}, Count: {Count}, Total: {Total}",
                        item.Status, item.Count, item.TotalAmount);
                }

                // Publish report to message bus
                var messagePublisher = scope.ServiceProvider.GetRequiredService<IMessagePublisher>();
                await messagePublisher.PublishAsync("settlement-exchange", "settlement.report",
                    new { GeneratedAt = DateTime.UtcNow, Summary = summary }, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Settlement Report Worker");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
