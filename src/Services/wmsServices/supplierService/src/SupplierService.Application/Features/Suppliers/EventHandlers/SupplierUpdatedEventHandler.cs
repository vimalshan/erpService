using MediatR;
using Microsoft.Extensions.Logging;
using SupplierService.Domain.Events;

namespace SupplierService.Application.Features.Suppliers.EventHandlers;

public class SupplierUpdatedEventHandler : INotificationHandler<SupplierUpdatedEvent>
{
    private readonly ILogger<SupplierUpdatedEventHandler> _logger;

    public SupplierUpdatedEventHandler(ILogger<SupplierUpdatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(SupplierUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event: Supplier updated - {Code}", notification.Supplier.Code);
        return Task.CompletedTask;
    }
}
