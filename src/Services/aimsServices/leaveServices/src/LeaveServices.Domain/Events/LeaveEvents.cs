using MediatR;

namespace LeaveServices.Domain.Events;

public sealed record LeaveAppliedEvent(
    long   LeaveDetailId,
    long   EmpSysId,
    long   LeaveId,
    DateTime From,
    DateTime To,
    decimal  AppliedDays) : INotification;

public sealed record LeaveApprovedEvent(
    long LeaveDetailId,
    long EmpSysId,
    long ApprovedBy) : INotification;

public sealed record LeaveRejectedEvent(
    long   LeaveDetailId,
    long   EmpSysId,
    long   RejectedBy,
    string Remarks) : INotification;

public sealed record LeaveCancelledEvent(
    long LeaveDetailId,
    long EmpSysId,
    long CancelledBy) : INotification;
