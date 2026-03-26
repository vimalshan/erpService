using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Swipes.Commands.RecordSwipe;

public sealed record RecordSwipeCommand(
    long EmployeeSysId,
    int GateNo,
    DateTime PunchTime,
    char PunchStatus,
    int? MachineNo,
    string? ReferenceNo,
    long UpdatedBy) : IRequest<SwipeDto>;
