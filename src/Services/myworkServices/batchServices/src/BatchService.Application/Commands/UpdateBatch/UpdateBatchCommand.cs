using MediatR;
using BatchService.Application.DTOs;

namespace BatchService.Application.Commands.UpdateBatch;

public sealed record UpdateBatchCommand(long BatchId, int MonthNo, long ModifiedBy) : IRequest<BatchDto>;
