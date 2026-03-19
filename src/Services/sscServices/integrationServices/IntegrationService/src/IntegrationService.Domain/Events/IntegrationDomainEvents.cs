using IntegrationService.Domain.Common;

namespace IntegrationService.Domain.Events;

public record PurchaseOrderCreatedEvent(long PoSeqId, string PoNumber) : DomainEvent;

public record PurchaseOrderUpdatedEvent(long PoSeqId, string PoNumber) : DomainEvent;

public record MaterialReceiptAddedEvent(long PoSeqId, long MrcSeqId, string MrcNumber) : DomainEvent;

public record VendorCreatedEvent(int VendorId, string VendorName) : DomainEvent;

public record VendorUpdatedEvent(int VendorId, string VendorName) : DomainEvent;

public record OrganizationUnitCreatedEvent(string OuId, string OuName) : DomainEvent;
