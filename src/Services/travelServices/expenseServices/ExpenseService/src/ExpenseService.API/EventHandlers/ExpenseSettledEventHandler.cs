using ExpenseService.Application.Interfaces;
using ExpenseService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseService.API.EventHandlers;

public class ExpenseSettledEventHandler : INotificationHandler<ExpenseSettledEvent>
{
    private readonly ILogger<ExpenseSettledEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public ExpenseSettledEventHandler(ILogger<ExpenseSettledEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(ExpenseSettledEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Expense settled - Request: {RequestNum}, Settlement: {Settlement}, Refund: {Refund}",
            notification.RequestNumber, notification.SettlementAmount, notification.RefundAmount);

        await _publisher.PublishAsync("expense.exchange", "expense.settled", notification, cancellationToken);
    }
}
