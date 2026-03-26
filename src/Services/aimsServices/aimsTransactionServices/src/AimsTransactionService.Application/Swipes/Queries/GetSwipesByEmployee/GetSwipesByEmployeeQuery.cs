using MediatR;
using AimsTransactionService.Application.DTOs;

namespace AimsTransactionService.Application.Swipes.Queries.GetSwipesByEmployee;

public sealed record GetSwipesByEmployeeQuery(
    long EmployeeSysId,
    DateTime FromDate,
    DateTime ToDate) : IRequest<IEnumerable<SwipeDto>>;
