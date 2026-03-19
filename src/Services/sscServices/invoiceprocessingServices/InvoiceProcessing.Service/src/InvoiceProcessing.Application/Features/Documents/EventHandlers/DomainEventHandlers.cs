using InvoiceProcessing.Domain.Events;
using InvoiceProcessing.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace InvoiceProcessing.Application.Features.Documents.EventHandlers;

public class DocumentCreatedEventHandler(ILogger<DocumentCreatedEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<DocumentCreatedEvent>
{
    public async Task Handle(DocumentCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document {DocumentId} created for Org {OrgId}, Type: {DocType}",
            notification.DocumentId, notification.OrgId, notification.DocumentType);

        await publisher.PublishAsync("invoice-processing", "document.created", notification, ct);
    }
}

public class DocumentSubmittedEventHandler(ILogger<DocumentSubmittedEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<DocumentSubmittedEvent>
{
    public async Task Handle(DocumentSubmittedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document {DocumentId} submitted for Org {OrgId}", notification.DocumentId, notification.OrgId);
        await publisher.PublishAsync("invoice-processing", "document.submitted", notification, ct);
    }
}

public class DocumentApprovedEventHandler(ILogger<DocumentApprovedEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<DocumentApprovedEvent>
{
    public async Task Handle(DocumentApprovedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document {DocumentId} approved by {ApprovedBy}", notification.DocumentId, notification.ApprovedBy);
        await publisher.PublishAsync("invoice-processing", "document.approved", notification, ct);
    }
}

public class DocumentCancelledEventHandler(ILogger<DocumentCancelledEventHandler> logger, IMessagePublisher publisher)
    : INotificationHandler<DocumentCancelledEvent>
{
    public async Task Handle(DocumentCancelledEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document {DocumentId} cancelled by {CancelledBy}", notification.DocumentId, notification.CancelledBy);
        await publisher.PublishAsync("invoice-processing", "document.cancelled", notification, ct);
    }
}

public class DocumentHoldEventHandler(ILogger<DocumentHoldEventHandler> logger)
    : INotificationHandler<DocumentHoldEvent>
{
    public Task Handle(DocumentHoldEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document {DocumentId} put on hold", notification.DocumentId);
        return Task.CompletedTask;
    }
}
