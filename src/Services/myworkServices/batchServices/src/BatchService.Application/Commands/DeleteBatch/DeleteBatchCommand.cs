using MediatR;

namespace BatchService.Application.Commands.DeleteBatch;

public sealed record DeleteBatchCommand(long BatchId) : IRequest;
