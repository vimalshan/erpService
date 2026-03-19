using MediatR;
using BatchAndEnvelopeService.Application.DTOs;

namespace BatchAndEnvelopeService.Application.Queries.Envelope;

public record GetEnvelopeByIdQuery(long EnvelopeId) : IRequest<EnvelopeDto?>;

public record GetAllEnvelopesQuery(int Page = 1, int PageSize = 20) : IRequest<IEnumerable<EnvelopeDto>>;

public record GetEnvelopesByTypeQuery(string EnvelopeType) : IRequest<IEnumerable<EnvelopeDto>>;
