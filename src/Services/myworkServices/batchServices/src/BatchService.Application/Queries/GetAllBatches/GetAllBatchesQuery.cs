using MediatR;
using BatchService.Application.DTOs;

namespace BatchService.Application.Queries.GetAllBatches;

public sealed record GetAllBatchesQuery : IRequest<IEnumerable<BatchDto>>;
