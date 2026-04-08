using MediatR;
using Microsoft.Extensions.Logging;
using SciTransactional.Application.Interfaces;
using SciTransactional.Domain.Events;

namespace SciTransactional.Application.EventHandlers;

public sealed class NavigationCreatedEventHandler(
    IRabbitMqPublisher publisher, ILogger<NavigationCreatedEventHandler> logger)
    : INotificationHandler<NavigationCreatedEvent>
{
    public async Task Handle(NavigationCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Navigation {RequestNum} created by {UserId}",
            notification.RequestNum, notification.UserId);
        await publisher.PublishAsync(
            "sci-transactional", "navigation.created", notification, cancellationToken);
    }
}

public sealed class NormCreatedEventHandler(
    IRabbitMqPublisher publisher, ILogger<NormCreatedEventHandler> logger)
    : INotificationHandler<NormCreatedEvent>
{
    public async Task Handle(NormCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Norm {NormNo} created", notification.NormNo);
        await publisher.PublishAsync(
            "sci-transactional", "norm.created", notification, cancellationToken);
    }
}

public sealed class NormClosedEventHandler(
    IRabbitMqPublisher publisher, ILogger<NormClosedEventHandler> logger)
    : INotificationHandler<NormClosedEvent>
{
    public async Task Handle(NormClosedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Norm {NormNo} closed on {Date}",
            notification.NormNo, notification.ClosureDate);
        await publisher.PublishAsync(
            "sci-transactional", "norm.closed", notification, cancellationToken);
    }
}

public sealed class LicenseCreatedEventHandler(
    IRabbitMqPublisher publisher, ILogger<LicenseCreatedEventHandler> logger)
    : INotificationHandler<LicenseCreatedEvent>
{
    public async Task Handle(LicenseCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: License {LicenseId} created", notification.LicenseId);
        await publisher.PublishAsync(
            "sci-transactional", "license.created", notification, cancellationToken);
    }
}

public sealed class LicenseUpdatedEventHandler(
    IRabbitMqPublisher publisher, ILogger<LicenseUpdatedEventHandler> logger)
    : INotificationHandler<LicenseUpdatedEvent>
{
    public async Task Handle(LicenseUpdatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: License {LicenseId} updated", notification.LicenseId);
        await publisher.PublishAsync(
            "sci-transactional", "license.updated", notification, cancellationToken);
    }
}
