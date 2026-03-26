namespace AimsTransactionService.Application.DTOs;

public sealed record SwipeDto(
    long SwipeId,
    long EmployeeSysId,
    DateTime PunchTime,
    string PunchStatus,
    int GateNo,
    string? MachineNo,
    string? ReferenceNo,
    string PullStatus,
    DateTime EnteredOn);
