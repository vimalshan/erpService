using MediatR;
using ReferenceDataService.Application.Interfaces;
using ReferenceDataService.Domain.Events;

namespace ReferenceDataService.API.EventHandlers;

public class LovMasterDeletedEventHandler : INotificationHandler<LovMasterDeletedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<LovMasterDeletedEventHandler> _logger;

    public LovMasterDeletedEventHandler(IMessagePublisher messagePublisher, ILogger<LovMasterDeletedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(LovMasterDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: LovMaster Deleted - {LovId}", notification.LovId);

        await _messagePublisher.PublishAsync("reference-data", "lov.master.deleted", new
        {
            notification.LovId,
            Timestamp = DateTime.UtcNow
        }, cancellationToken);
    }
}
