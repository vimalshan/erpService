using ExpenseService.Application.Interfaces;
using ExpenseService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ExpenseService.API.EventHandlers;

public class ExpenseRecordedEventHandler : INotificationHandler<ExpenseRecordedEvent>
{
    private readonly ILogger<ExpenseRecordedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public ExpenseRecordedEventHandler(ILogger<ExpenseRecordedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(ExpenseRecordedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Expense recorded - Request: {RequestNum}, Serial: {SerialNum}, Budget: {Budget}",
            notification.RequestNumber, notification.SerialNumber, notification.BudgetAmount);

        await _publisher.PublishAsync("expense.exchange", "expense.recorded", notification, cancellationToken);
    }
}
