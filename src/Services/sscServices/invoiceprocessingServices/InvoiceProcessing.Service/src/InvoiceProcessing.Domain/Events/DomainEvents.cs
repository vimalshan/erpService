using InvoiceProcessing.Domain.Common;

namespace InvoiceProcessing.Domain.Events;

public record DocumentCreatedEvent(long DocumentId, string OrgId, string DocumentType) : IDomainEvent;
public record DocumentSubmittedEvent(long DocumentId, string OrgId) : IDomainEvent;
public record DocumentApprovedEvent(long DocumentId, long ApprovedBy) : IDomainEvent;
public record DocumentCancelledEvent(long DocumentId, long CancelledBy) : IDomainEvent;
public record DocumentHoldEvent(long DocumentId) : IDomainEvent;
public record InvoiceProcessedEvent(long DocumentId, string InvoiceNo, long Amount) : IDomainEvent;
public record InvoiceValidatedEvent(long DocumentId, long AllocationId) : IDomainEvent;
public record PaymentCompletedEvent(long DocumentId, long PaymentId, decimal Amount) : IDomainEvent;
public record AllocationChangedEvent(long DocumentId, long AllocationId, string Action) : IDomainEvent;
public record CorrespondenceCreatedEvent(long DocumentId, long CorrespondenceId, long HoldCategory) : IDomainEvent;
