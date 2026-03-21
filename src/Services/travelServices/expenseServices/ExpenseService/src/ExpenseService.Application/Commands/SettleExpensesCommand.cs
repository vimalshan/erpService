using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Commands;

public record SettleExpensesCommand : IRequest<SettlementResultDto>
{
    public long RequestNumber { get; init; }
}
