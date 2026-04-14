using MediatR;
using Microsoft.Extensions.Logging;
using SupplierService.Application.Interfaces;
using SupplierService.Domain.Events;

namespace SupplierService.Application.Features.Suppliers.EventHandlers;

public class SupplierCreatedEventHandler : INotificationHandler<SupplierCreatedEvent>
{
    private readonly ILogger<SupplierCreatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public SupplierCreatedEventHandler(ILogger<SupplierCreatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(SupplierCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Supplier created - {Code} ({Name})",
            notification.Supplier.Code, notification.Supplier.Name);

        await _publisher.PublishAsync("supplier.events", "supplier.created", new
        {
            SupplierId = notification.Supplier.SupplierId,
            Code = notification.Supplier.Code,
            Name = notification.Supplier.Name,
            notification.OccurredOn
        }, cancellationToken);
    }
}
