using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SettlementService.Domain.Enums;
using SettlementService.Domain.Interfaces;

namespace SettlementService.Functions.BackgroundTasks;

public class SettlementExpiryWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<SettlementExpiryWorker> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    public SettlementExpiryWorker(IServiceProvider serviceProvider, ILogger<SettlementExpiryWorker> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Settlement Expiry Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var settlements = await unitOfWork.Settlements.GetAllAsync(stoppingToken);
                var expiredSettlements = settlements
                    .Where(s => s.StStatus == SettlementStatus.Pending
                        && s.StSetDate.HasValue
                        && s.StSetDate.Value.AddDays(90) < DateTime.UtcNow);

                foreach (var settlement in expiredSettlements)
                {
                    settlement.Reject(0, "Auto-rejected: settlement expired after 90 days");
                    await unitOfWork.Settlements.UpdateAsync(settlement, stoppingToken);
                    _logger.LogInformation("Auto-rejected expired settlement {SettlementNumber}", settlement.StSetNum);
                }

                await unitOfWork.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Settlement Expiry Worker");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
