using ContributionService.Application.Commands.ContributionBatch;
using MediatR;

namespace ContributionService.Functions.BackgroundTasks;

public class MonthlyContributionProcessor(
    IServiceScopeFactory scopeFactory,
    ILogger<MonthlyContributionProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Monthly Contribution Processor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;
                // Run on the 1st of each month
                if (now.Day == 1 && now.Hour == 2 && now.Minute == 0)
                {
                    var monthYear = now.AddMonths(-1).ToString("yyyy-MM");
                    logger.LogInformation("Processing monthly contribution for {MonthYear}", monthYear);

                    using var scope = scopeFactory.CreateScope();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    var result = await mediator.Send(
                        new ProcessMonthlyContributionCommand(monthYear, 0), stoppingToken);

                    logger.LogInformation("Monthly contribution processed: {Message}", result.Message);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in monthly contribution processing");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
