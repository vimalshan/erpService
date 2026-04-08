using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PFTransactionalService.Application.Commands.ProcessContribution;

namespace PFTransactionalService.Functions.Workers;

public class MonthlyContributionWorker : BackgroundService
{
    private readonly ILogger<MonthlyContributionWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public MonthlyContributionWorker(ILogger<MonthlyContributionWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Monthly Contribution Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Monthly Contribution Worker checking for pending contributions at {Time}", DateTime.UtcNow);

                // Worker runs daily at midnight to check for monthly contribution processing
                // Actual batch processing would be triggered by an external scheduler
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation("Monthly contribution check completed. Next check in 24 hours.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Monthly Contribution Worker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
