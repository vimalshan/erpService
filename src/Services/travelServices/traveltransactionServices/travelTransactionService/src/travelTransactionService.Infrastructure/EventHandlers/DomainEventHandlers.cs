using MediatR;
using Microsoft.Extensions.Logging;
using travelTransactionService.Domain.Events;
using travelTransactionService.Domain.Interfaces;

namespace travelTransactionService.Infrastructure.EventHandlers;

public class VendorCreatedEventHandler : INotificationHandler<VendorCreatedEvent>
{
    private readonly ILogger<VendorCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public VendorCreatedEventHandler(
        ILogger<VendorCreatedEventHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(VendorCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Vendor {VendorId} ({VendorName}) created",
            notification.VendorId, notification.VendorName);

        await _messagePublisher.PublishAsync("vendor-created", new
        {
            notification.VendorId,
            notification.VendorName,
            notification.OccurredOn
        }, cancellationToken);
    }
}

public class TaxMasterCreatedEventHandler : INotificationHandler<TaxMasterCreatedEvent>
{
    private readonly ILogger<TaxMasterCreatedEventHandler> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public TaxMasterCreatedEventHandler(
        ILogger<TaxMasterCreatedEventHandler> logger,
        IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    public async Task Handle(TaxMasterCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Tax master {TaxType} created for vendor {VendorId}",
            notification.TaxType, notification.VendorId);

        await _messagePublisher.PublishAsync("tax-master-created", new
        {
            notification.VendorId,
            notification.TaxType,
            notification.OccurredOn
        }, cancellationToken);
    }
}

public class JaiInterfaceLineCreatedEventHandler : INotificationHandler<JaiInterfaceLineCreatedEvent>
{
    private readonly ILogger<JaiInterfaceLineCreatedEventHandler> _logger;

    public JaiInterfaceLineCreatedEventHandler(ILogger<JaiInterfaceLineCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(JaiInterfaceLineCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("JAI interface line created: Transaction {TransactionNum}, Line {LineNum}",
            notification.TransactionNum, notification.TransactionLineNum);

        return Task.CompletedTask;
    }
}
