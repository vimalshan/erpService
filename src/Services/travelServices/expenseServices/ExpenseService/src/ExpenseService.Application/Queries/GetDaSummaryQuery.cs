using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Queries;

public record GetDaSummaryQuery : IRequest<DaSummaryDto?>
{
    public long RequestId { get; init; }
}
