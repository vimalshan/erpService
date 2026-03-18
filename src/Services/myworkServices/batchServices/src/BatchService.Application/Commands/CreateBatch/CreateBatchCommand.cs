using MediatR;
using BatchService.Application.DTOs;

namespace BatchService.Application.Commands.CreateBatch;

public sealed record CreateBatchCommand(long BatchId, int MonthNo, long ModifiedBy) : IRequest<BatchDto>;
