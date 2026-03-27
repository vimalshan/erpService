using LoanManagement.Domain.Events;
using LoanManagement.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Application.EventHandlers;

public class LoanCreatedEventHandler : INotificationHandler<LoanCreatedEvent>
{
    private readonly ILogger<LoanCreatedEventHandler> _logger;
    private readonly IEventPublisher _publisher;

    public LoanCreatedEventHandler(ILogger<LoanCreatedEventHandler> logger, IEventPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(LoanCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Loan created. LoanId={LoanId}, Key={LoanKey}, Amount={Amount}",
            notification.LoanId, notification.LoanKey, notification.LoanAmount);

        await _publisher.PublishAsync("loan.events", "loan.created", new
        {
            notification.LoanId,
            notification.LoanKey,
            notification.LoanAmount,
            notification.OccurredOn
        });
    }
}
