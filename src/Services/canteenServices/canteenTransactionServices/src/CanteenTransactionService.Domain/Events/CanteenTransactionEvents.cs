using CanteenTransactionService.Domain.Common;

namespace CanteenTransactionService.Domain.Events;

public sealed record CanteenTransactionRecordedEvent(
    long SerialNumber,
    long EmployeeSysId,
    long? ItemCode,
    decimal? EmployeeContribution,
    decimal? EmployerContribution) : IDomainEvent;

public sealed record DailyAvailedProcessedEvent(
    long SerialNumber,
    long CompanyCode,
    long EmployeeSysId,
    long? ItemCode) : IDomainEvent;

public sealed record MisBatchSubmittedEvent(
    long BatchNumber,
    long CompanyCode,
    string EmployeeNumber) : IDomainEvent;

public sealed record TransactionCancelledEvent(
    long SerialNumber,
    long EmployeeSysId) : IDomainEvent;
