using MediatR;
using DemandManagement.Application.DTOs;

namespace DemandManagement.Application.Queries;

public record GetAllDemandsQuery : IRequest<IEnumerable<DemandDto>>;
public record GetDemandByIdQuery(long DemandId) : IRequest<DemandDto?>;
