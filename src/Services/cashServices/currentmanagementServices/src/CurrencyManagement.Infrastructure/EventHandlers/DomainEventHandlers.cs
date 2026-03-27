using MediatR;
using Microsoft.Extensions.Logging;
using CurrencyManagement.Application.Common.Interfaces;
using CurrencyManagement.Domain.Events;

namespace CurrencyManagement.Infrastructure.EventHandlers;

public class CurrencyCreatedEventHandler : INotificationHandler<CurrencyCreatedDomainEvent>
{
    private readonly ILogger<CurrencyCreatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public CurrencyCreatedEventHandler(ILogger<CurrencyCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(CurrencyCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Currency created - ID={CurrencyId}, Name={Name}, Symbol={Symbol}",
            notification.CurrencyId, notification.Name, notification.Symbol);
        await _publisher.PublishAsync(notification, cancellationToken);
    }
}

public class CurrencyUpdatedEventHandler : INotificationHandler<CurrencyUpdatedDomainEvent>
{
    private readonly ILogger<CurrencyUpdatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public CurrencyUpdatedEventHandler(ILogger<CurrencyUpdatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(CurrencyUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Currency updated - ID={CurrencyId}, Name={Name}, Symbol={Symbol}",
            notification.CurrencyId, notification.Name, notification.Symbol);
        await _publisher.PublishAsync(notification, cancellationToken);
    }
}

public class CurrencyDeletedEventHandler : INotificationHandler<CurrencyDeletedDomainEvent>
{
    private readonly ILogger<CurrencyDeletedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public CurrencyDeletedEventHandler(ILogger<CurrencyDeletedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(CurrencyDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Currency deleted - ID={CurrencyId}", notification.CurrencyId);
        await _publisher.PublishAsync(notification, cancellationToken);
    }
}

public class ExchangeRateSetEventHandler : INotificationHandler<ExchangeRateSetDomainEvent>
{
    private readonly ILogger<ExchangeRateSetEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public ExchangeRateSetEventHandler(ILogger<ExchangeRateSetEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(ExchangeRateSetDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Exchange rate set - RateId={RateId}, From={From}, To={To}, Rate={Rate}",
            notification.RateId, notification.FromCurrencyId, notification.ToCurrencyId, notification.Rate);
        await _publisher.PublishAsync(notification, cancellationToken);
    }
}

public class OrganizationCurrencyMappedEventHandler : INotificationHandler<OrganizationCurrencyMappedDomainEvent>
{
    private readonly ILogger<OrganizationCurrencyMappedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public OrganizationCurrencyMappedEventHandler(ILogger<OrganizationCurrencyMappedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(OrganizationCurrencyMappedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Organization currency mapped - OrgId={OrgId}, CurrencyId={CurrencyId}",
            notification.OrganizationId, notification.CurrencyId);
        await _publisher.PublishAsync(notification, cancellationToken);
    }
}
