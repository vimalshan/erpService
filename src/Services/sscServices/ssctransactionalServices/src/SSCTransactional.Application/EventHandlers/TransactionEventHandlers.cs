using MediatR;
using Microsoft.Extensions.Logging;
using SSCTransactional.Application.Interfaces;
using SSCTransactional.Domain.Events;

namespace SSCTransactional.Application.EventHandlers;

public class AllocationCreatedEventHandler : INotificationHandler<AllocationCreatedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AllocationCreatedEventHandler> _logger;

    public AllocationCreatedEventHandler(IMessagePublisher publisher, ILogger<AllocationCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AllocationCreatedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] AllocationCreated: {AllocationId} for Doc {DocId}", notification.AllocationId, notification.DocId);
        await _publisher.PublishAsync("transaction.exchange", "allocation.created", notification);
    }
}

public class AllocationCompletedEventHandler : INotificationHandler<AllocationCompletedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<AllocationCompletedEventHandler> _logger;

    public AllocationCompletedEventHandler(IMessagePublisher publisher, ILogger<AllocationCompletedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(AllocationCompletedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] AllocationCompleted: {AllocationId}", notification.AllocationId);
        await _publisher.PublishAsync("transaction.exchange", "allocation.completed", notification);
    }
}

public class CorrespondenceCreatedEventHandler : INotificationHandler<CorrespondenceCreatedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CorrespondenceCreatedEventHandler> _logger;

    public CorrespondenceCreatedEventHandler(IMessagePublisher publisher, ILogger<CorrespondenceCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CorrespondenceCreatedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] CorrespondenceCreated: {CorrespondenceId} for Doc {DocId}", notification.CorrespondenceId, notification.DocId);
        await _publisher.PublishAsync("transaction.exchange", "correspondence.created", notification);
    }
}

public class CorrespondenceReleasedEventHandler : INotificationHandler<CorrespondenceReleasedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CorrespondenceReleasedEventHandler> _logger;

    public CorrespondenceReleasedEventHandler(IMessagePublisher publisher, ILogger<CorrespondenceReleasedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CorrespondenceReleasedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] CorrespondenceReleased: {CorrespondenceId}", notification.CorrespondenceId);
        await _publisher.PublishAsync("transaction.exchange", "correspondence.released", notification);
    }
}

public class RescanRequestedEventHandler : INotificationHandler<RescanRequestedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<RescanRequestedEventHandler> _logger;

    public RescanRequestedEventHandler(IMessagePublisher publisher, ILogger<RescanRequestedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(RescanRequestedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] RescanRequested: {RescanId} for Doc {DocId}", notification.RescanId, notification.DocId);
        await _publisher.PublishAsync("transaction.exchange", "rescan.requested", notification);
    }
}

public class DocumentRevokedEventHandler : INotificationHandler<DocumentRevokedDomainEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<DocumentRevokedEventHandler> _logger;

    public DocumentRevokedEventHandler(IMessagePublisher publisher, ILogger<DocumentRevokedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(DocumentRevokedDomainEvent notification, CancellationToken ct)
    {
        _logger.LogInformation("[DomainEvent] DocumentRevoked: {RevokeId} for Doc {DocId}", notification.RevokeId, notification.DocId);
        await _publisher.PublishAsync("transaction.exchange", "document.revoked", notification);
    }
}
