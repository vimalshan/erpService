using MediatR;
using ReferenceDataService.Application.Interfaces;
using ReferenceDataService.Domain.Events;

namespace ReferenceDataService.API.EventHandlers;

public class LovMasterCreatedEventHandler : INotificationHandler<LovMasterCreatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<LovMasterCreatedEventHandler> _logger;

    public LovMasterCreatedEventHandler(IMessagePublisher messagePublisher, ILogger<LovMasterCreatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(LovMasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: LovMaster Created - {LovId}", notification.LovMaster.LovId);

        await _messagePublisher.PublishAsync("reference-data", "lov.master.created", new
        {
            notification.LovMaster.LovId,
            notification.LovMaster.LovType,
            notification.LovMaster.LovName,
            Timestamp = DateTime.UtcNow
        }, cancellationToken);
    }
}
