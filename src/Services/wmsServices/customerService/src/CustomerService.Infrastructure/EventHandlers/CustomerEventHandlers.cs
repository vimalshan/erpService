using CustomerService.Application.Interfaces;
using CustomerService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CustomerService.Infrastructure.EventHandlers;

public class CustomerCreatedEventHandler : INotificationHandler<CustomerCreatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CustomerCreatedEventHandler> _logger;

    public CustomerCreatedEventHandler(IMessagePublisher publisher, ILogger<CustomerCreatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CustomerCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer created: {CustomerId} - {Code}", notification.Customer.CustomerId, notification.Customer.Code);

        await _publisher.PublishAsync("customer.exchange", "customer.created", new
        {
            notification.Customer.CustomerId,
            notification.Customer.Code,
            notification.Customer.Name,
            Action = "Created"
        }, cancellationToken);
    }
}

public class CustomerUpdatedEventHandler : INotificationHandler<CustomerUpdatedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CustomerUpdatedEventHandler> _logger;

    public CustomerUpdatedEventHandler(IMessagePublisher publisher, ILogger<CustomerUpdatedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CustomerUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer updated: {CustomerId}", notification.Customer.CustomerId);

        await _publisher.PublishAsync("customer.exchange", "customer.updated", new
        {
            notification.Customer.CustomerId,
            notification.Customer.Code,
            notification.Customer.Name,
            Action = "Updated"
        }, cancellationToken);
    }
}

public class CustomerDeletedEventHandler : INotificationHandler<CustomerDeletedEvent>
{
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<CustomerDeletedEventHandler> _logger;

    public CustomerDeletedEventHandler(IMessagePublisher publisher, ILogger<CustomerDeletedEventHandler> logger)
    {
        _publisher = publisher;
        _logger = logger;
    }

    public async Task Handle(CustomerDeletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer deleted: {CustomerId}", notification.CustomerId);

        await _publisher.PublishAsync("customer.exchange", "customer.deleted", new
        {
            notification.CustomerId,
            Action = "Deleted"
        }, cancellationToken);
    }
}

public class CustomerActivatedEventHandler : INotificationHandler<CustomerActivatedEvent>
{
    private readonly ILogger<CustomerActivatedEventHandler> _logger;

    public CustomerActivatedEventHandler(ILogger<CustomerActivatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerActivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer activated: {CustomerId}", notification.CustomerId);
        return Task.CompletedTask;
    }
}

public class CustomerDeactivatedEventHandler : INotificationHandler<CustomerDeactivatedEvent>
{
    private readonly ILogger<CustomerDeactivatedEventHandler> _logger;

    public CustomerDeactivatedEventHandler(ILogger<CustomerDeactivatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(CustomerDeactivatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Customer deactivated: {CustomerId}", notification.CustomerId);
        return Task.CompletedTask;
    }
}
