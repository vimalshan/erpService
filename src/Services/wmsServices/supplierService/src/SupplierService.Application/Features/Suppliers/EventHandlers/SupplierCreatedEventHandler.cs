using MediatR;
using Microsoft.Extensions.Logging;
using SupplierService.Domain.Events;

namespace SupplierService.Application.Features.Suppliers.EventHandlers;

public class SupplierCreatedEventHandler : INotificationHandler<SupplierCreatedEvent>
{
    private readonly ILogger<SupplierCreatedEventHandler> _logger;

    public SupplierCreatedEventHandler(ILogger<SupplierCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SupplierCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Supplier created - {Code} ({Name})",
            notification.Supplier.Code, notification.Supplier.Name);
        return Task.CompletedTask;
    }
}
