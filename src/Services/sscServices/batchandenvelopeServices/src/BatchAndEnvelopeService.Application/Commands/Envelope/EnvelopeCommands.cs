using MediatR;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Application.Commands.Envelope;

public record CreateEnvelopeCommand(
    string EnvelopeType,
    long CreatedBy,
    long LocationId,
    List<(int DocumentId, string Type)> Documents
) : IRequest<EnvelopeDto>;

public record ConfirmEnvelopeCommand(long EnvelopeId, long ConfirmedBy) : IRequest<EnvelopeDto>;

public record CancelEnvelopeCommand(long EnvelopeId, long CancelledBy) : IRequest<EnvelopeDto>;
