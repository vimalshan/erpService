namespace MobileExpenseManagement.Application.EventHandlers;

using MediatR;
using MobileExpenseManagement.Domain.Entities;
using Microsoft.Extensions.Logging;

/// <summary>
/// Event handler for expense created domain event
/// </summary>
public class ExpenseCreatedEventHandler : INotificationHandler<ExpenseCreatedDomainEvent>
{
    private readonly ILogger<ExpenseCreatedEventHandler> _logger;

    public ExpenseCreatedEventHandler(ILogger<ExpenseCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ExpenseCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            $"Expense created: ExpenseId={notification.ExpenseId}, TripId={notification.TripId}, Amount={notification.Amount}");

        // TODO: Publish to RabbitMQ, send notifications, update aggregates, etc.
        return Task.CompletedTask;
    }
}

/// <summary>
/// Event handler for expense updated domain event
/// </summary>
public class ExpenseUpdatedEventHandler : INotificationHandler<ExpenseUpdatedDomainEvent>
{
    private readonly ILogger<ExpenseUpdatedEventHandler> _logger;

    public ExpenseUpdatedEventHandler(ILogger<ExpenseUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ExpenseUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            $"Expense updated: ExpenseId={notification.ExpenseId}, NewAmount={notification.NewAmount}");

        // TODO: Publish to RabbitMQ, send notifications, etc.
        return Task.CompletedTask;
    }
}

/// <summary>
/// Event handler for expense deleted domain event
/// </summary>
public class ExpenseDeletedEventHandler : INotificationHandler<ExpenseDeletedDomainEvent>
{
    private readonly ILogger<ExpenseDeletedEventHandler> _logger;

    public ExpenseDeletedEventHandler(ILogger<ExpenseDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ExpenseDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Expense deleted: ExpenseId={notification.ExpenseId}");

        // TODO: Publish to RabbitMQ, send notifications, cleanup files, etc.
        return Task.CompletedTask;
    }
}
