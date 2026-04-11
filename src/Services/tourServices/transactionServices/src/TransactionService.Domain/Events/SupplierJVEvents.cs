using TransactionService.Domain.Common;

namespace TransactionService.Domain.Events;

public sealed record SupplierJVCreatedEvent(
    Guid EventId,
    long JvId,
    long VendorId,
    string JvType,
    decimal NetAmount,
    long CreatedBy,
    DateTime OccurredOn) : IDomainEvent;

public sealed record SupplierJVPostedEvent(
    Guid EventId,
    long JvId,
    string? OracleRefNo,
    long PostedBy,
    DateTime OccurredOn) : IDomainEvent;
