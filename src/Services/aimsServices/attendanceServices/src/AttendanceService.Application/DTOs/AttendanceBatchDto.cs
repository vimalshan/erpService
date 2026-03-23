namespace AttendanceService.Application.DTOs;

public record AttendanceBatchDto(
    long BatchId,
    int MonthFrom,
    int MonthTo,
    int YearFrom,
    int YearEnd,
    string Status,
    long CreatedBy,
    DateTime CreatedOn,
    DateTime LastModifiedOn);

public record ProcessMonthlyAttendanceRequest(
    DateTime MonthStart,
    DateTime MonthEnd,
    long ProcessedBy);
