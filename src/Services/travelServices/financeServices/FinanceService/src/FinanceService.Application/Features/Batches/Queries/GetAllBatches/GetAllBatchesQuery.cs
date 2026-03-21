using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Batches.Queries.GetAllBatches;

public record GetAllBatchesQuery : IRequest<List<BatchDto>>
{
    public string? UnitCode { get; init; }
}
