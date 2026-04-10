using MediatR;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Application.Commands.Envelope;

public record EnvelopeDocumentInput(int DocumentId, string Type);

public record CreateEnvelopeCommand(
    string EnvelopeType,
    long CreatedBy,
    long LocationId,
    List<EnvelopeDocumentInput> Documents
) : IRequest<EnvelopeDto>;

public record ConfirmEnvelopeCommand(long EnvelopeId, long ConfirmedBy) : IRequest<EnvelopeDto>;

public record CancelEnvelopeCommand(long EnvelopeId, long CancelledBy) : IRequest<EnvelopeDto>;
