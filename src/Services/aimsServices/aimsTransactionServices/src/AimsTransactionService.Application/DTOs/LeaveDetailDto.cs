namespace AimsTransactionService.Application.DTOs;

public sealed record LeaveDetailDto(
    long LeaveDetailId,
    long EmployeeSysId,
    int LeaveId,
    DateTime FromDate,
    DateTime ToDate,
    decimal LeaveDays,
    string? Reason,
    string Status,
    long AppliedBy,
    DateTime AppliedOn);
