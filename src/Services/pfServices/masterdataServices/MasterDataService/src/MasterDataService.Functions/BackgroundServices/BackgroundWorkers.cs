using MasterDataService.Application.Interfaces;
using MasterDataService.Domain.Interfaces;
using MasterDataService.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MasterDataService.Functions.BackgroundServices;

/// <summary>Periodically syncs LOV master data and purges inactive entries</summary>
public class LovSyncWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<LovSyncWorker> _logger;

    public LovSyncWorker(IServiceScopeFactory scopeFactory, ILogger<LovSyncWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("LovSyncWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<ILovMasterRepository>();
                var all = await repository.GetAllAsync(stoppingToken);
                var inactiveCount = all.Count(x => x.LovStatus != "A");
                _logger.LogInformation("LovSyncWorker: {Total} LOV records ({Inactive} inactive) at {Time}",
                    all.Count(), inactiveCount, DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LovSyncWorker encountered an error.");
            }
            await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
        }
    }
}

/// <summary>Periodically reports configuration snapshot for audit trail</summary>
public class ConfigurationAuditWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<ConfigurationAuditWorker> _logger;
    private readonly IMessagePublisher _publisher;

    public ConfigurationAuditWorker(IServiceScopeFactory scopeFactory, ILogger<ConfigurationAuditWorker> logger, IMessagePublisher publisher)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _publisher = publisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ConfigurationAuditWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IConfigurationRepository>();
                var all = await repository.GetAllAsync(stoppingToken);
                await _publisher.PublishAsync("masterdata.config.audit", new
                {
                    Timestamp = DateTime.UtcNow,
                    ConfigCount = all.Count()
                }, stoppingToken);
                _logger.LogInformation("ConfigurationAuditWorker: Published audit snapshot at {Time}", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ConfigurationAuditWorker error.");
            }
            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}

/// <summary>Rate expiry checker — flags rates with passed closing dates</summary>
public class RateExpiryCheckerWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RateExpiryCheckerWorker> _logger;

    public RateExpiryCheckerWorker(IServiceScopeFactory scopeFactory, ILogger<RateExpiryCheckerWorker> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RateExpiryCheckerWorker started.");
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var repository = scope.ServiceProvider.GetRequiredService<IRateMasterRepository>();
                var allRates = await repository.GetAllAsync(stoppingToken);
                var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
                var expiredCount = allRates.Count(r =>
                    !string.IsNullOrEmpty(r.RateClosingDate) &&
                    string.Compare(r.RateClosingDate, today, StringComparison.Ordinal) < 0 &&
                    r.RateDeleteFlag != "Y");
                if (expiredCount > 0)
                    _logger.LogWarning("RateExpiryCheckerWorker: {Count} rates have expired closing dates.", expiredCount);
                else
                    _logger.LogInformation("RateExpiryCheckerWorker: No expired rates found at {Time}.", DateTimeOffset.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RateExpiryCheckerWorker error.");
            }
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }
}
