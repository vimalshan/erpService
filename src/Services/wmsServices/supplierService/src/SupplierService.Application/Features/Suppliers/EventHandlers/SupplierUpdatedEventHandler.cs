using MediatR;
using Microsoft.Extensions.Logging;
using SupplierService.Application.Interfaces;
using SupplierService.Domain.Events;

namespace SupplierService.Application.Features.Suppliers.EventHandlers;

public class SupplierUpdatedEventHandler : INotificationHandler<SupplierUpdatedEvent>
{
    private readonly ILogger<SupplierUpdatedEventHandler> _logger;
    private readonly IMessagePublisher _publisher;

    public SupplierUpdatedEventHandler(ILogger<SupplierUpdatedEventHandler> logger, IMessagePublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }

    public async Task Handle(SupplierUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Supplier updated - {Code}", notification.Supplier.Code);

        await _publisher.PublishAsync("supplier.events", "supplier.updated", new
        {
            SupplierId = notification.Supplier.SupplierId,
            Code = notification.Supplier.Code,
            Name = notification.Supplier.Name,
            notification.OccurredOn
        }, cancellationToken);
    }
}
