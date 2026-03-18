using MediatR;
using SwipeTransactionService.Application.DTOs;

namespace SwipeTransactionService.Application.Features.CanteenPunch.Queries;

public sealed record GetPunchByEmployeeDateQuery(
    long EmployeeSysId,
    DateTime Date) : IRequest<CanteenPunchDto?>;

public sealed record GetPunchesByEmployeeQuery(
    long EmployeeSysId,
    DateTime From,
    DateTime To) : IRequest<IEnumerable<CanteenPunchDto>>;
