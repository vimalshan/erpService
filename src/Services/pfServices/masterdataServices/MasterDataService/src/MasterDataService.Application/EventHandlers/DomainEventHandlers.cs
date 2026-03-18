using MasterDataService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace MasterDataService.Application.EventHandlers;

public class LovStatusChangedEventHandler : INotificationHandler<LovStatusChangedEvent>
{
    private readonly ILogger<LovStatusChangedEventHandler> _logger;
    public LovStatusChangedEventHandler(ILogger<LovStatusChangedEventHandler> logger) => _logger = logger;

    public Task Handle(LovStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("LOV {LovId} status changed to {Status} at {OccurredOn}",
            notification.LovId, notification.NewStatus, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class RateChangedEventHandler : INotificationHandler<RateChangedEvent>
{
    private readonly ILogger<RateChangedEventHandler> _logger;
    public RateChangedEventHandler(ILogger<RateChangedEventHandler> logger) => _logger = logger;

    public Task Handle(RateChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rate {TrustCode}/{RateId} changed from {OldValue} to {NewValue} at {OccurredOn}",
            notification.TrustCode, notification.RateId, notification.OldValue, notification.NewValue, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class ConfigurationChangedEventHandler : INotificationHandler<ConfigurationChangedEvent>
{
    private readonly ILogger<ConfigurationChangedEventHandler> _logger;
    public ConfigurationChangedEventHandler(ILogger<ConfigurationChangedEventHandler> logger) => _logger = logger;

    public Task Handle(ConfigurationChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Configuration key '{Key}' changed at {OccurredOn}",
            notification.ConfigKey, notification.OccurredOn);
        return Task.CompletedTask;
    }
}

public class FinancialYearClosedEventHandler : INotificationHandler<FinancialYearClosedEvent>
{
    private readonly ILogger<FinancialYearClosedEventHandler> _logger;
    public FinancialYearClosedEventHandler(ILogger<FinancialYearClosedEventHandler> logger) => _logger = logger;

    public Task Handle(FinancialYearClosedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Financial Year {SerialNumber} closed at {OccurredOn}",
            notification.SerialNumber, notification.OccurredOn);
        return Task.CompletedTask;
    }
}
