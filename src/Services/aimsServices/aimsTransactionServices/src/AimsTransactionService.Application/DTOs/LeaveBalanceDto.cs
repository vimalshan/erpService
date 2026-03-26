namespace AimsTransactionService.Application.DTOs;

public sealed record LeaveBalanceDto(
    long EmployeeSysId,
    int LeaveId,
    decimal Balance);
