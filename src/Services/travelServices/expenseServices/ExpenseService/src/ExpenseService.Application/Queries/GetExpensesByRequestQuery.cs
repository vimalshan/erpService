using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Queries;

public record GetExpensesByRequestQuery : IRequest<IReadOnlyList<TravelExpenseDto>>
{
    public long RequestNumber { get; init; }
}
