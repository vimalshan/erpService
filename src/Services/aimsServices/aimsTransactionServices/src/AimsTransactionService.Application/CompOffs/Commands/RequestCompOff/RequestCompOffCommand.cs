using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.CompOffs.Commands.RequestCompOff;

public sealed record RequestCompOffCommand(
    long EmployeeSysId,
    decimal HoursRequested,
    long RequestedBy) : IRequest<CompOffDto>;
