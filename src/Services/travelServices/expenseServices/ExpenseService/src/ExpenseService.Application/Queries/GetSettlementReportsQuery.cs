using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Queries;

public record GetSettlementReportsQuery : IRequest<IReadOnlyList<SettlementDto>>
{
    public long RequestNumber { get; init; }
}
