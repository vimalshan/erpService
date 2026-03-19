using MediatR;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Application.Commands.Batch;

public record CreateBatchCommand(
    long CreatedBy,
    long LocationId,
    long ReceivedBy,
    string PodNo,
    string? CourierName,
    List<int> EnvelopeIds
) : IRequest<BatchDto>;

public record ConfirmBatchCommand(long BatchId, long ConfirmedBy) : IRequest<BatchDto>;

public record CancelBatchCommand(long BatchId, long CancelledBy) : IRequest<BatchDto>;
