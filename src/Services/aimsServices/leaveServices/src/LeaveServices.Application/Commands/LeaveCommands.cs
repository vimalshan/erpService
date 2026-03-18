using MediatR;
using LeaveServices.Application.DTOs;

namespace LeaveServices.Application.Commands.Leave;

// ── Apply Leave ────────────────────────────────────────────────────────────────
public record ApplyLeaveCommand(
    long     EmpSysId,
    long     LeaveId,
    DateTime FromDate,
    DateTime ToDate,
    string   AppType,
    int      TimeUnitId,
    decimal  AppliedDays,
    string?  Reason,
    long     AppliedBy) : IRequest<long>;

// ── Approve / Reject / Cancel ─────────────────────────────────────────────────
public record ApproveLeaveCommand(
    long   LeaveDetailId,
    string Status,
    string? Remarks,
    long   ApprovedBy) : IRequest<Unit>;

public record CancelLeaveCommand(
    long LeaveDetailId,
    long CancelledBy) : IRequest<Unit>;

// ── Leave Master ──────────────────────────────────────────────────────────────
public record CreateLeaveMasterCommand(
    string Description,
    char   GenderSpecific,
    char   ApplicableForAll,
    int    MaxDaysPL,
    char   Encashable,
    char   CarryForward,
    long   CreatedBy) : IRequest<long>;

public record UpdateLeaveMasterCommand(
    long   LeaveId,
    string Description,
    char   GenderSpecific,
    char   ApplicableForAll,
    int    MaxDaysPL,
    char   Encashable,
    char   CarryForward,
    long   ModifiedBy) : IRequest<Unit>;

// ── Leave Credit ──────────────────────────────────────────────────────────────
public record CreditLeaveCommand(
    long    EmpSysId,
    long    LeaveId,
    char    Flag,
    int     Year,
    decimal Opening,
    decimal Credited,
    long    ModifiedBy) : IRequest<long>;

// ── Comp-Off ──────────────────────────────────────────────────────────────────
public record AddCompOffCommand(
    long     EmpSysId,
    DateTime CompOffDate,
    long     CreatedBy) : IRequest<long>;
