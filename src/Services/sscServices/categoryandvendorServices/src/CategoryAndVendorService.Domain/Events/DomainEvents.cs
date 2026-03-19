using CategoryAndVendorService.Domain.Common;

namespace CategoryAndVendorService.Domain.Events;

public sealed record MainCategoryCreatedEvent(long MainCatId, string MainCatName) : DomainEvent;
public sealed record MainCategoryUpdatedEvent(long MainCatId, string MainCatName) : DomainEvent;
public sealed record SubCategoryCreatedEvent(long SubCatId, string SubCatName, long MainCatId) : DomainEvent;
public sealed record VendorDocumentCreatedEvent(long VndDocId, long VendorId) : DomainEvent;
public sealed record VendorDocumentApprovedEvent(long VndDocId, long VendorId, long ApprovedBy) : DomainEvent;
public sealed record VendorDocumentRejectedEvent(long VndDocId, long VendorId, long RejectedBy) : DomainEvent;
