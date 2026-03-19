using IntegrationService.Application.Interfaces;
using IntegrationService.Domain.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IntegrationService.Infrastructure.Services;

public class PurchaseOrderCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<PurchaseOrderCreatedEventHandler> logger) : INotificationHandler<PurchaseOrderCreatedEvent>
{
    public async Task Handle(PurchaseOrderCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: PO {PoNumber} created (SeqId: {PoSeqId})", notification.PoNumber, notification.PoSeqId);
        await publisher.PublishAsync("integration.events", "po.created", notification, cancellationToken);
    }
}

public class VendorCreatedEventHandler(
    IMessagePublisher publisher,
    ILogger<VendorCreatedEventHandler> logger) : INotificationHandler<VendorCreatedEvent>
{
    public async Task Handle(VendorCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: Vendor {VendorName} created (Id: {VendorId})", notification.VendorName, notification.VendorId);
        await publisher.PublishAsync("integration.events", "vendor.created", notification, cancellationToken);
    }
}

public class OrganizationUnitCreatedEventHandler(
    ILogger<OrganizationUnitCreatedEventHandler> logger) : INotificationHandler<OrganizationUnitCreatedEvent>
{
    public Task Handle(OrganizationUnitCreatedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: OU {OuName} created (Id: {OuId})", notification.OuName, notification.OuId);
        return Task.CompletedTask;
    }
}

public class MaterialReceiptAddedEventHandler(
    IMessagePublisher publisher,
    ILogger<MaterialReceiptAddedEventHandler> logger) : INotificationHandler<MaterialReceiptAddedEvent>
{
    public async Task Handle(MaterialReceiptAddedEvent notification, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event: MRC {MrcNumber} added to PO {PoSeqId}", notification.MrcNumber, notification.PoSeqId);
        await publisher.PublishAsync("integration.events", "mrc.added", notification, cancellationToken);
    }
}
