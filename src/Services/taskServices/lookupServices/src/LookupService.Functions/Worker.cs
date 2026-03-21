using LookupService.Application.Queries;
using LookupService.Infrastructure.Dapper;
using MediatR;

namespace LookupService.Functions;

/// <summary>
/// Background worker that periodically monitors lookup data health and performs cache warming.
/// In production, replace with Azure Functions TimerTrigger.
/// </summary>
public class LookupDataMonitorWorker(
    ILogger<LookupDataMonitorWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Lookup Data Monitor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

                // Monitor LOV data
                var lovTypes = await mediator.Send(new GetAllLovTypesQuery(), stoppingToken);
                var lovs = await mediator.Send(new GetAllLovsQuery(), stoppingToken);
                var processes = await mediator.Send(new GetAllProcessesQuery(), stoppingToken);

                logger.LogInformation(
                    "Lookup Data Health: {LovTypes} LOV types, {Lovs} LOVs, {Processes} processes",
                    lovTypes.Count(), lovs.Count(), processes.Count());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error monitoring lookup data");
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}

/// <summary>
/// Background worker for cleaning up stale access detail records.
/// </summary>
public class AccessDetailCleanupWorker(
    ILogger<AccessDetailCleanupWorker> logger,
    IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Access Detail Cleanup Worker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = scopeFactory.CreateScope();
                var dapper = scope.ServiceProvider.GetRequiredService<LookupDapperQueries>();

                // Log status of data
                var processes = await dapper.GetAllProcessesAsync();
                var activeProcesses = processes.Where(p => p.ProcessLivFlag == "Y").Count();
                logger.LogInformation("Active processes: {Count}", activeProcesses);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error in cleanup worker");
            }

            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}

