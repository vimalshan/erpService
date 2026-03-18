using DeductionService.Domain.Common;

namespace DeductionService.Domain.Events;

public record DeductionCreatedEvent(
    long SystemId,
    long? EmployeeNumber,
    decimal? Amount) : DomainEvent;

public record DeductionCancelledEvent(
    long SystemId,
    long? EmployeeNumber,
    long CancelledByUserId) : DomainEvent;

public record MonthlyDeductionProcessedEvent(
    long EmployeeSystemId,
    string MonthYear,
    decimal TotalAmount,
    long ProcessedByUserId) : DomainEvent;

public record DeductionAccessGrantedEvent(
    long AccessNumber,
    long UnitCode,
    string? DeductionType) : DomainEvent;

public record DeductionAccessRevokedEvent(
    long AccessNumber,
    long UnitCode) : DomainEvent;
