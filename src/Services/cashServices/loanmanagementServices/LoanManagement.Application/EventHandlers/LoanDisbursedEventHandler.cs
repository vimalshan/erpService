using LoanManagement.Domain.Events;
using LoanManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Application.EventHandlers;

public class LoanDisbursedEventHandler : INotificationHandler<LoanDisbursedEvent>
{
    private readonly ILogger<LoanDisbursedEventHandler> _logger;
    private readonly IEventPublisher _publisher;

    public LoanDisbursedEventHandler(ILogger<LoanDisbursedEventHandler> logger, IEventPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(LoanDisbursedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Loan disbursed. LoanId={LoanId}, DisbursementId={DisbId}, Amount={Amount}",
            notification.LoanId, notification.DisbursementId, notification.Amount);

        await _publisher.PublishAsync("loan.events", "loan.disbursed", new
        {
            notification.LoanId,
            notification.DisbursementId,
            notification.Amount,
            notification.OccurredOn
        });
    }
}
