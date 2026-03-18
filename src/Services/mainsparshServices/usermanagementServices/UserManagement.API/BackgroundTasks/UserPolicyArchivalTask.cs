using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UserManagement.Infrastructure.Dapper;

namespace UserManagement.API.BackgroundTasks;

/// <summary>
/// Azure Functions-style background task: runs on a timer to archive
/// expired user policies and generate audit summaries.
/// Hosted as an IHostedService in the API for local/container deployments;
/// can be extracted to a dedicated Azure Functions Isolated Worker project.
/// </summary>
public class UserPolicyArchivalTask(
    IServiceScopeFactory scopeFactory,
    ILogger<UserPolicyArchivalTask> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(24);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("UserPolicyArchivalTask started. Interval: {Interval}", Interval);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Interval, stoppingToken);

            try
            {
                await RunAsync(stoppingToken);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "UserPolicyArchivalTask encountered an error.");
            }
        }
    }

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dapper = scope.ServiceProvider.GetRequiredService<UserManagementDapperContext>();

        var summary = await dapper.GetActiveUserPoliciesSummaryAsync(cancellationToken);
        var count = summary.Count();

        logger.LogInformation(
            "[{Time}] UserPolicyArchivalTask: found {Count} active policies.",
            DateTime.UtcNow, count);

        // Extend here: move expired policies to archive table, send email digest, etc.
    }
}

/// <summary>
/// Background task: sends newsletter digest to opted-in contacts.
/// Equivalent to an Azure Function with Timer trigger.
/// </summary>
public class NewsletterDigestTask(
    IServiceScopeFactory scopeFactory,
    ILogger<NewsletterDigestTask> logger) : BackgroundService
{
    private static readonly TimeSpan Interval = TimeSpan.FromHours(6);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(Interval, stoppingToken);

            try
            {
                using var scope = scopeFactory.CreateScope();
                // Retrieve newsletter subscribers and dispatch digest
                logger.LogInformation("[{Time}] NewsletterDigestTask: dispatching newsletter.", DateTime.UtcNow);
            }
            catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
            {
                logger.LogError(ex, "NewsletterDigestTask error.");
            }
        }
    }
}
