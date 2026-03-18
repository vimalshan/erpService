using MediatR;
using Microsoft.Extensions.Logging;
using TrustService.Domain.Events;

namespace TrustService.Application.Features.EventHandlers;

public class TrustCreatedEventHandler : INotificationHandler<TrustCreatedEvent>
{
    private readonly ILogger<TrustCreatedEventHandler> _logger;

    public TrustCreatedEventHandler(ILogger<TrustCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TrustCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Trust created - Code: {TrustCode}, Name: {TrustName}",
            notification.TrustCode, notification.TrustName);
        return Task.CompletedTask;
    }
}

public class TrustUpdatedEventHandler : INotificationHandler<TrustUpdatedEvent>
{
    private readonly ILogger<TrustUpdatedEventHandler> _logger;

    public TrustUpdatedEventHandler(ILogger<TrustUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TrustUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Trust updated - Code: {TrustCode}, Name: {TrustName}",
            notification.TrustCode, notification.TrustName);
        return Task.CompletedTask;
    }
}

public class TrustClosedEventHandler : INotificationHandler<TrustClosedEvent>
{
    private readonly ILogger<TrustClosedEventHandler> _logger;

    public TrustClosedEventHandler(ILogger<TrustClosedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TrustClosedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Trust closed - Code: {TrustCode}", notification.TrustCode);
        return Task.CompletedTask;
    }
}

public class TrustStatusChangedEventHandler : INotificationHandler<TrustStatusChangedEvent>
{
    private readonly ILogger<TrustStatusChangedEventHandler> _logger;

    public TrustStatusChangedEventHandler(ILogger<TrustStatusChangedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TrustStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Trust status changed - Code: {TrustCode}, NewStatus: {NewStatus}",
            notification.TrustCode, notification.NewStatus);
        return Task.CompletedTask;
    }
}

public class TrustFundTypeAddedEventHandler : INotificationHandler<TrustFundTypeAddedEvent>
{
    private readonly ILogger<TrustFundTypeAddedEventHandler> _logger;

    public TrustFundTypeAddedEventHandler(ILogger<TrustFundTypeAddedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TrustFundTypeAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Fund type added - Trust: {TrustCode}, Type: {FundType}, Name: {FundName}",
            notification.TrustCode, notification.FundType, notification.FundName);
        return Task.CompletedTask;
    }
}

public class TrustUnitAddedEventHandler : INotificationHandler<TrustUnitAddedEvent>
{
    private readonly ILogger<TrustUnitAddedEventHandler> _logger;

    public TrustUnitAddedEventHandler(ILogger<TrustUnitAddedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(TrustUnitAddedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Unit added - Trust: {TrustCode}, Unit: {UnitCode}, Name: {UnitName}",
            notification.TrustCode, notification.UnitCode, notification.UnitName);
        return Task.CompletedTask;
    }
}
