using LoanManagement.Domain.Events;
using LoanManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Application.EventHandlers;

public class LoanRepaymentScheduledEventHandler : INotificationHandler<LoanRepaymentScheduledEvent>
{
    private readonly ILogger<LoanRepaymentScheduledEventHandler> _logger;
    private readonly IEventPublisher _publisher;

    public LoanRepaymentScheduledEventHandler(ILogger<LoanRepaymentScheduledEventHandler> logger, IEventPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(LoanRepaymentScheduledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Repayment scheduled. LoanId={LoanId}, RepaymentId={RepayId}, Amount={Amount}",
            notification.LoanId, notification.RepaymentId, notification.Amount);

        await _publisher.PublishAsync("loan.events", "loan.repayment.scheduled", new
        {
            notification.LoanId,
            notification.RepaymentId,
            notification.RepayDate,
            notification.Amount,
            notification.OccurredOn
        });
    }
}
