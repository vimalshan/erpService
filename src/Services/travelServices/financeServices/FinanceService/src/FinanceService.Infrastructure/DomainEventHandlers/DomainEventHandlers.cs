using FinanceService.Application.Common.Interfaces;
using FinanceService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FinanceService.Infrastructure.DomainEventHandlers;

public class BatchCreatedEventHandler : INotificationHandler<BatchCreatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<BatchCreatedEventHandler> _logger;

    public BatchCreatedEventHandler(IMessagePublisher messagePublisher, ILogger<BatchCreatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(BatchCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Batch {BatchNumber} created for unit {UnitCode}",
            notification.BatchNumber, notification.UnitCode);

        await _messagePublisher.PublishAsync(notification, "finance", "batch.created", cancellationToken);
    }
}

public class BatchApprovedEventHandler : INotificationHandler<BatchApprovedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<BatchApprovedEventHandler> _logger;

    public BatchApprovedEventHandler(IMessagePublisher messagePublisher, ILogger<BatchApprovedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(BatchApprovedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Batch {BatchNumber} approved for unit {UnitCode}",
            notification.BatchNumber, notification.UnitCode);

        await _messagePublisher.PublishAsync(notification, "finance", "batch.approved", cancellationToken);
    }
}

public class PaymentProcessedEventHandler : INotificationHandler<PaymentProcessedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<PaymentProcessedEventHandler> _logger;

    public PaymentProcessedEventHandler(IMessagePublisher messagePublisher, ILogger<PaymentProcessedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(PaymentProcessedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Payment {TransactionNumber} processed for amount {Amount}",
            notification.TransactionNumber, notification.Amount);

        await _messagePublisher.PublishAsync(notification, "finance", "payment.processed", cancellationToken);
    }
}

public class InvoiceCreatedEventHandler : INotificationHandler<InvoiceCreatedEvent>
{
    private readonly IMessagePublisher _messagePublisher;
    private readonly ILogger<InvoiceCreatedEventHandler> _logger;

    public InvoiceCreatedEventHandler(IMessagePublisher messagePublisher, ILogger<InvoiceCreatedEventHandler> logger)
    {
        _messagePublisher = messagePublisher;
        _logger = logger;
    }

    public async Task Handle(InvoiceCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Invoice {InvoiceId} created with number {InvoiceNum}",
            notification.InvoiceId, notification.InvoiceNum);

        await _messagePublisher.PublishAsync(notification, "finance", "invoice.created", cancellationToken);
    }
}
