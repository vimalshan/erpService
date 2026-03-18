using LoanManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Application.EventHandlers;

public class LoanCreatedEventHandler : INotificationHandler<LoanCreatedEvent>
{
    private readonly ILogger<LoanCreatedEventHandler> _logger;

    public LoanCreatedEventHandler(ILogger<LoanCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LoanCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Loan created. LoanId={LoanId}, Key={LoanKey}, Amount={Amount}",
            notification.LoanId, notification.LoanKey, notification.LoanAmount);

        return Task.CompletedTask;
    }
}
