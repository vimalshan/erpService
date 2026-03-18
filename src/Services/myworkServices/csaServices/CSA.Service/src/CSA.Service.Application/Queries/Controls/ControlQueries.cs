using CSA.Service.Application.DTOs;
using MediatR;

namespace CSA.Service.Application.Queries.Controls;

public record GetControlByIdQuery(long ControlId) : IRequest<ControlDto?>;
public record GetAllControlsQuery : IRequest<IEnumerable<ControlDto>>;
public record GetControlsByProcessQuery(long ProcessId) : IRequest<IEnumerable<ControlDto>>;
public record GetEvidencesByControlQuery(long ControlId) : IRequest<IEnumerable<EvidenceDto>>;
