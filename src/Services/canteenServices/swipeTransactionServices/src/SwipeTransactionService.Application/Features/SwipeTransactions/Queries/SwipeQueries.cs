using MediatR;
using SwipeTransactionService.Application.DTOs;

namespace SwipeTransactionService.Application.Features.SwipeTransactions.Queries;

public sealed record GetSwipesByEmployeeQuery(
    string EmployeeNumber,
    DateTime From,
    DateTime To) : IRequest<IEnumerable<SwipeCardUploadDto>>;

public sealed record GetPendingSwipesQuery() : IRequest<IEnumerable<SwipeCardUploadDto>>;
