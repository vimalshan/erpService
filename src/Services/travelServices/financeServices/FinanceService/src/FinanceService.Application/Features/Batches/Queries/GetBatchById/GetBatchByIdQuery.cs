using MediatR;
using FinanceService.Application.DTOs;

namespace FinanceService.Application.Features.Batches.Queries.GetBatchById;

public record GetBatchByIdQuery(string UnitCode, decimal BatchNumber) : IRequest<BatchDto>;
