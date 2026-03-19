using MediatR;
using ProductionManagement.Application.Queries.ProductionPlans;
using ProductionManagement.Domain.Interfaces;

namespace ProductionManagement.Functions.Workers;

/// <summary>
/// Background worker that checks for expired production plans and closes them.
/// Runs every hour.
/// </summary>
public class ProductionPlanExpiryWorker : BackgroundService
{
    private readonly ILogger<ProductionPlanExpiryWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromHours(1);

    public ProductionPlanExpiryWorker(ILogger<ProductionPlanExpiryWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProductionPlanExpiryWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var plans = await unitOfWork.ProductionPlans.GetAllAsync(stoppingToken);
                var expiredPlans = plans.Where(p => p.PlanClosureDate is null && p.PlanStartDate < (decimal)DateTime.UtcNow.Subtract(TimeSpan.FromDays(365)).Ticks).ToList();

                foreach (var plan in expiredPlans)
                {
                    plan.ClosePlan(0); // System-initiated closure
                    await unitOfWork.ProductionPlans.UpdateAsync(plan, stoppingToken);
                }

                if (expiredPlans.Count > 0)
                {
                    await unitOfWork.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Closed {Count} expired production plans", expiredPlans.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProductionPlanExpiryWorker");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}

/// <summary>
/// Background worker that generates daily production summary reports.
/// Runs every 24 hours.
/// </summary>
public class ProductionReportGeneratorWorker : BackgroundService
{
    private readonly ILogger<ProductionReportGeneratorWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public ProductionReportGeneratorWorker(ILogger<ProductionReportGeneratorWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ProductionReportGeneratorWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var plants = await unitOfWork.ProductionPlants.GetAllAsync(stoppingToken);
                var plans = await unitOfWork.ProductionPlans.GetAllAsync(stoppingToken);

                _logger.LogInformation(
                    "Daily Production Report: {PlantCount} plants, {PlanCount} active plans, Generated at {Time}",
                    plants.Count,
                    plans.Count(p => p.PlanClosureDate is null),
                    DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProductionReportGeneratorWorker");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}

/// <summary>
/// Background worker that cleans up expired norms.
/// Runs every 12 hours.
/// </summary>
public class NormsCleanupWorker : BackgroundService
{
    private readonly ILogger<NormsCleanupWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly TimeSpan _interval = TimeSpan.FromHours(12);

    public NormsCleanupWorker(ILogger<NormsCleanupWorker> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("NormsCleanupWorker started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var norms = await unitOfWork.Norms.GetAllAsync(stoppingToken);
                var openNormsCount = norms.Count(n => n.NormClsDate is null);
                var closedNormsCount = norms.Count(n => n.NormClsDate is not null);

                _logger.LogInformation("Norms Status: {Open} open, {Closed} closed", openNormsCount, closedNormsCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in NormsCleanupWorker");
            }

            await Task.Delay(_interval, stoppingToken);
        }
    }
}
