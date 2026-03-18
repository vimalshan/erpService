using MediatR;
using SwipeTransactionService.Application.DTOs;

namespace SwipeTransactionService.Application.Features.CanteenPunch.Commands;

public sealed record RecordPunchCommand(
    long EmployeeSysId,
    long CanteenUnit,
    char PunchType,   // 'I' = Check-in, 'O' = Check-out
    DateTime? PunchTime = null) : IRequest<CanteenPunchDto>;
