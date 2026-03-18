using MediatR;
using BatchService.Application.DTOs;

namespace BatchService.Application.Queries.GetBatch;

public sealed record GetBatchQuery(long BatchId) : IRequest<BatchDto?>;
