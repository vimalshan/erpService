using AccountingService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AccountingService.Application.EventHandlers;

public class TransactionCreatedEventHandler : INotificationHandler<TransactionCreatedEvent>
{
    private readonly ILogger<TransactionCreatedEventHandler> _logger;

    public TransactionCreatedEventHandler(ILogger<TransactionCreatedEventHandler> logger)
        => _logger = logger;

    public Task Handle(TransactionCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Transaction created. TrustCode={TrustCode}, TransactionId={Id}, Amount={Amount}",
            notification.Transaction.TdTrustCode,
            notification.Transaction.TransactionId,
            notification.Transaction.TdAmount);

        return Task.CompletedTask;
    }
}
