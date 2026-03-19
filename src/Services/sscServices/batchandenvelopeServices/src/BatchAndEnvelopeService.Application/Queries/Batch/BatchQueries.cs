using MediatR;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Application.Queries.Batch;

public record GetBatchByIdQuery(long BatchId) : IRequest<BatchDto?>;

public record GetAllBatchesQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<BatchDto>>;

public record GetBatchesByLocationQuery(long LocationId) : IRequest<IEnumerable<BatchDto>>;
