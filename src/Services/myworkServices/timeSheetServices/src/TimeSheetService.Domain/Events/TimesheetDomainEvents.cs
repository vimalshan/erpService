using TimeSheetService.Domain.Common;

namespace TimeSheetService.Domain.Events;

public sealed record TimesheetSubmittedEvent(
    long TimeId, long EmployeeSysId, DateTime TimeDate, long TotalHours) : IDomainEvent;

public sealed record TimesheetUpdatedEvent(
    long TimeId, long EmployeeSysId, long TotalHours) : IDomainEvent;

public sealed record TimesheetDeletedEvent(
    long TimeId, long EmployeeSysId) : IDomainEvent;

public sealed record TcTimesheetSubmittedEvent(
    long TimeId, long EmployeeSysId, DateTime TimeDate, long TotalHours) : IDomainEvent;
