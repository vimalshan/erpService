using SwipeTransactionService.Domain.Common;

namespace SwipeTransactionService.Domain.Events;

public sealed record SwipeTransactionRecordedEvent(
    long CompanyCode,
    string EmployeeNumber,
    DateTime SwipeTime,
    long ItemCode,
    long Quantity) : IDomainEvent;

public sealed record EmployeePunchedInEvent(
    long EmployeeSysId,
    long CanteenUnit,
    DateTime PunchTime) : IDomainEvent;

public sealed record EmployeePunchedOutEvent(
    long EmployeeSysId,
    long CanteenUnit,
    DateTime PunchTime,
    decimal WorkHours) : IDomainEvent;

public sealed record CanteenItemAvailedEvent(
    long EmployeeSysId,
    long ItemCode,
    decimal EmployeeContribution,
    decimal EmployerContribution,
    DateTime AvailedAt) : IDomainEvent;

public sealed record CanteenTransactionRecordedEvent(
    long EmployeeSysId,
    long ItemCode,
    decimal EmployeeShare,
    decimal EmployerShare) : IDomainEvent;
