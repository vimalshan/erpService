using MediatR;
using ReferenceDataService.Application.Interfaces;
using ReferenceDataService.Domain.Events;

namespace ReferenceDataService.API.EventHandlers;

public class LovMasterUpdatedEventHandler : INotificationHandler<LovMasterUpdatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<LovMasterUpdatedEventHandler> _logger;

    public LovMasterUpdatedEventHandler(IMessagePublisher messagePublisher, ILogger<LovMasterUpdatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(LovMasterUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: LovMaster Updated - {LovId}", notification.LovMaster.LovId);

        await _messagePublisher.PublishAsync("reference-data", "lov.master.updated", new
        {
            notification.LovMaster.LovId,
            notification.LovMaster.LovType,
            notification.LovMaster.LovName,
            Timestamp = DateTime.UtcNow
        }, cancellationToken);
    }
}
