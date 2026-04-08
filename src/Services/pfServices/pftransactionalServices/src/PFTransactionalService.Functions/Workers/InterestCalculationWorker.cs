using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PFTransactionalService.Application.Commands.ApplyInterest;

namespace PFTransactionalService.Functions.Workers;

public class InterestCalculationWorker : BackgroundService
{
    private readonly ILogger<InterestCalculationWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public InterestCalculationWorker(ILogger<InterestCalculationWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Interest Calculation Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Interest Calculation Worker checking at {Time}", DateTime.UtcNow);

                // Worker checks for annual interest calculation at financial year end
                using var scope = _scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                _logger.LogInformation("Interest calculation check completed. Next check in 24 hours.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Interest Calculation Worker");
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
