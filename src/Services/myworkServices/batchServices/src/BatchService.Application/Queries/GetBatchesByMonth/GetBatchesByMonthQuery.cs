using MediatR;
using BatchService.Application.DTOs;

namespace BatchService.Application.Queries.GetBatchesByMonth;

public sealed record GetBatchesByMonthQuery(int MonthNo) : IRequest<IEnumerable<BatchDto>>;
