using Microsoft.EntityFrameworkCore;
using ReimbursementService.Domain.Enums;
using ReimbursementService.Infrastructure.Persistence;

namespace ReimbursementService.Functions;

/// <summary>
/// Background worker that runs scheduled tasks for the Reimbursement service.
/// Acts as the Azure Functions substitute in self-hosted mode.
/// </summary>
public class Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Reimbursement background worker started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunDailyTasksAsync(stoppingToken);
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                logger.LogError(ex, "Error in background worker.");
            }

            // Run every hour
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }

    private async Task RunDailyTasksAsync(CancellationToken ct)
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Task 1: Auto-expire old SUBMITTED reimbursements (> 30 days with no action)
        var cutoff = DateTime.UtcNow.AddDays(-30);
        var stale = await context.ReimTran
            .Where(r => r.Status == ReimbursementStatus.Submitted && r.CreatedOn < cutoff)
            .ToListAsync(ct);

        foreach (var item in stale)
            logger.LogWarning("Stale reimbursement detected: {RefNo} — submitted {CreatedOn}",
                item.ReimRefNo, item.CreatedOn);

        // Task 2: Report summary of today's paid reimbursements
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var paidToday = await context.ReimTran
            .Where(r => r.Status == ReimbursementStatus.Paid && r.PaymentDate == today)
            .CountAsync(ct);

        logger.LogInformation("Reimbursements paid today: {Count}", paidToday);
    }
}

