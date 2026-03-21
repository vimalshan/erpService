using PurchaseOrderService.Domain.Common;

namespace PurchaseOrderService.Domain.Events;

public sealed record PurchaseOrderCreatedEvent(string PoNumber, int SupplierId, int WarehouseId) : DomainEvent;

public sealed record PurchaseOrderConfirmedEvent(string PoNumber) : DomainEvent;

public sealed record PurchaseOrderCompletedEvent(string PoNumber) : DomainEvent;

public sealed record PurchaseOrderCancelledEvent(string PoNumber) : DomainEvent;

public sealed record PurchaseOrderLineReceivedEvent(string PoNumber, int LineNumber, decimal QuantityReceived) : DomainEvent;
