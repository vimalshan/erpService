using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Queries;

public record GetExpenseByIdQuery : IRequest<TravelExpenseDto?>
{
    public long RequestNumber { get; init; }
    public long SerialNumber { get; init; }
}
