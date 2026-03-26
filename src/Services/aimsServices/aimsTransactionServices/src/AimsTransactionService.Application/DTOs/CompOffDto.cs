namespace AimsTransactionService.Application.DTOs;

public sealed record CompOffDto(
    long CompOffId,
    long EmployeeSysId,
    DateTime WorkDate,
    decimal RequestedHours,
    string Status,
    long RequestedBy,
    DateTime RequestedOn);
