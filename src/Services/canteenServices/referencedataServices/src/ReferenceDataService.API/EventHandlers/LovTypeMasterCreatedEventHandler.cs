using MediatR;
using ReferenceDataService.Application.Interfaces;
using ReferenceDataService.Domain.Events;

namespace ReferenceDataService.API.EventHandlers;

public class LovTypeMasterCreatedEventHandler : INotificationHandler<LovTypeMasterCreatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<LovTypeMasterCreatedEventHandler> _logger;

    public LovTypeMasterCreatedEventHandler(IMessagePublisher messagePublisher, ILogger<LovTypeMasterCreatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(LovTypeMasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: LovTypeMaster Created - {Code}", notification.LovTypeMaster.LovTypeCode);

        await _messagePublisher.PublishAsync("reference-data", "lov.typemaster.created", new
        {
            notification.LovTypeMaster.LovTypeCode,
            notification.LovTypeMaster.LovTypeName,
            Timestamp = DateTime.UtcNow
        }, cancellationToken);
    }
}
