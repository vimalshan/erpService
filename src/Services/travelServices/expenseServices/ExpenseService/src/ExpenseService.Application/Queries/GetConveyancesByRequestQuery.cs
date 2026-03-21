using ExpenseService.Application.DTOs;
using MediatR;

namespace ExpenseService.Application.Queries;

public record GetConveyancesByRequestQuery : IRequest<IReadOnlyList<ConveyanceDto>>
{
    public long RequestNumber { get; init; }
}
