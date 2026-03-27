using LoanManagement.Domain.Events;
using LoanManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Application.EventHandlers;

public class LoanStatusChangedEventHandler : INotificationHandler<LoanStatusChangedEvent>
{
    private readonly ILogger<LoanStatusChangedEventHandler> _logger;
    private readonly IEventPublisher _publisher;

    public LoanStatusChangedEventHandler(ILogger<LoanStatusChangedEventHandler> logger, IEventPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(LoanStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Loan status changed. LoanId={LoanId}, NewStatus={Status}",
            notification.LoanId, notification.NewStatus);

        await _publisher.PublishAsync("loan.events", "loan.status.changed", new
        {
            notification.LoanId,
            notification.NewStatus,
            notification.OccurredOn
        });
    }
}
