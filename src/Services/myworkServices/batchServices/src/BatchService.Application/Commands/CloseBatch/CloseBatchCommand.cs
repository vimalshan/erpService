using MediatR;

namespace BatchService.Application.Commands.CloseBatch;

public sealed record CloseBatchCommand(long BatchId, long ModifiedBy) : IRequest;
