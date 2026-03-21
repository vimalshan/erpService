using MediatR;

namespace ExpenseService.Application.Commands;

public record DeleteExpenseCommand : IRequest<bool>
{
    public long RequestNumber { get; init; }
    public long SerialNumber { get; init; }
}
