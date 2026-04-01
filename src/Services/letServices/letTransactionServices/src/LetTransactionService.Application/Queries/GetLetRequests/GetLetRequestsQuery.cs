using LetTransactionService.Application.DTOs;
using MediatR;

namespace LetTransactionService.Application.Queries.GetLetRequests;

public record GetLetRequestsQuery(int Page = 1, int PageSize = 20, string? EmployeeUserId = null)
    : IRequest<IEnumerable<LetSummaryDto>>;
