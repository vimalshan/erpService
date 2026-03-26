namespace AimsTransactionService.Application.DTOs;

public sealed record AttendanceSummaryDto(
    long SummaryId,
    long EmployeeSysId,
    DateTime MonthStart,
    DateTime MonthEnd,
    int WorkingDays,
    int PresentDays,
    int AbsentDays,
    decimal OvertimeHours,
    decimal LopDays);
