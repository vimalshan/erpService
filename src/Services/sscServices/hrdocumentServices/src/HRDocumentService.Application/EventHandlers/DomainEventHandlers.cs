using HRDocumentService.Application.Interfaces;
using HRDocumentService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace HRDocumentService.Application.EventHandlers;

public sealed class DocumentCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<DocumentCreatedEventHandler> logger)
    : INotificationHandler<DocumentCreatedEvent>
{
    public async Task Handle(DocumentCreatedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document created: {DocId}, DocNo: {DocNo}", notification.DocId, notification.DocNo);
        await publisher.PublishAsync("hr-documents", "document.created", notification, ct);
    }
}

public sealed class DocumentApprovedEventHandler(
    IMessagePublisher publisher,
    ILogger<DocumentApprovedEventHandler> logger)
    : INotificationHandler<DocumentApprovedEvent>
{
    public async Task Handle(DocumentApprovedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document approved: {DocId} by {ApprovedBy}", notification.DocId, notification.ApprovedBy);
        await publisher.PublishAsync("hr-documents", "document.approved", notification, ct);
    }
}

public sealed class DocumentRejectedEventHandler(
    IMessagePublisher publisher,
    ILogger<DocumentRejectedEventHandler> logger)
    : INotificationHandler<DocumentRejectedEvent>
{
    public async Task Handle(DocumentRejectedEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document rejected: {DocId} by {RejectedBy}", notification.DocId, notification.RejectedBy);
        await publisher.PublishAsync("hr-documents", "document.rejected", notification, ct);
    }
}

public sealed class DocumentCancelledEventHandler(
    IMessagePublisher publisher,
    ILogger<DocumentCancelledEventHandler> logger)
    : INotificationHandler<DocumentCancelledEvent>
{
    public async Task Handle(DocumentCancelledEvent notification, CancellationToken ct)
    {
        logger.LogInformation("Document cancelled: {DocId} by {CancelledBy}", notification.DocId, notification.CancelledBy);
        await publisher.PublishAsync("hr-documents", "document.cancelled", notification, ct);
    }
}
