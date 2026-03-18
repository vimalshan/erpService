using LoanManagement.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LoanManagement.Application.EventHandlers;

public class LoanStatusChangedEventHandler : INotificationHandler<LoanStatusChangedEvent>
{
    private readonly ILogger<LoanStatusChangedEventHandler> _logger;

    public LoanStatusChangedEventHandler(ILogger<LoanStatusChangedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(LoanStatusChangedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Domain Event: Loan status changed. LoanId={LoanId}, NewStatus={Status}",
            notification.LoanId, notification.NewStatus);

        return Task.CompletedTask;
    }
}
