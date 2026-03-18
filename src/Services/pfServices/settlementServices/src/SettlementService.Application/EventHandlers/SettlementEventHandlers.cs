using MediatR;
using SettlementService.Domain.Events;
using SettlementService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace SettlementService.Application.EventHandlers;

public class SettlementCreatedEventHandler : INotificationHandler<SettlementCreatedEvent>
{
    private readonly ILogger<SettlementCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public SettlementCreatedEventHandler(ILogger<SettlementCreatedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(SettlementCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Settlement {SettlementNumber} created for member {MemberNo}, amount: {Amount}",
            notification.SettlementNumber, notification.MemberNo, notification.Amount);

        await _messagePublisher.PublishAsync("settlement-exchange", "settlement.created", notification, cancellationToken);
    }
}

public class SettlementApprovedEventHandler : INotificationHandler<SettlementApprovedEvent>
{
    private readonly ILogger<SettlementApprovedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public SettlementApprovedEventHandler(ILogger<SettlementApprovedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(SettlementApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Settlement {SettlementNumber} approved by {ApprovedBy}",
            notification.SettlementNumber, notification.ApprovedBy);

        await _messagePublisher.PublishAsync("settlement-exchange", "settlement.approved", notification, cancellationToken);
    }
}

public class SettlementCompletedEventHandler : INotificationHandler<SettlementCompletedEvent>
{
    private readonly ILogger<SettlementCompletedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public SettlementCompletedEventHandler(ILogger<SettlementCompletedEventHandler> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(SettlementCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Settlement {SettlementNumber} completed", notification.SettlementNumber);

        await _messagePublisher.PublishAsync("settlement-exchange", "settlement.completed", notification, cancellationToken);
    }
}
