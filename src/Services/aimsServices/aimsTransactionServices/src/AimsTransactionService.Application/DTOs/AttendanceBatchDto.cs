namespace AimsTransactionService.Application.DTOs;

public sealed record AttendanceBatchDto(
    long BatchId,
    DateTime MonthStart,
    DateTime MonthEnd,
    string Status,
    int EmployeesProcessed,
    long CreatedBy,
    DateTime CreatedOn);
