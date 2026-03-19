using MediatR;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Application.Queries.Norms;

public record GetAllNormsQuery : IRequest<IReadOnlyList<NormsMainDto>>;
public record GetNormByIdQuery(long NormNo) : IRequest<NormsMainDto?>;
