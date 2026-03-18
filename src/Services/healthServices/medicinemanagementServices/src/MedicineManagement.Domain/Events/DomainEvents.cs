using MedicineManagement.Domain.Common;
using MedicineManagement.Domain.Entities;

namespace MedicineManagement.Domain.Events;

public sealed record MedicineTypeCreatedEvent(MedicineType MedicineType) : IDomainEvent;
public sealed record MedicineCreatedEvent(Medicine Medicine) : IDomainEvent;
public sealed record StockTransactionCreatedEvent(MedicineCredit MedicineCredit) : IDomainEvent;
public sealed record MedicineIssuedEvent(MedicineIssue MedicineIssue) : IDomainEvent;
public sealed record PurchaseCreatedEvent(PurchaseMain Purchase) : IDomainEvent;
public sealed record LowStockDetectedEvent(string MedicineCode, string MedicineName, long CurrentStock, decimal MinLevel) : IDomainEvent;
public sealed record MedicineExpiredEvent(string MedicineCode, string LotNumber, DateTime ExpiryDate) : IDomainEvent;
