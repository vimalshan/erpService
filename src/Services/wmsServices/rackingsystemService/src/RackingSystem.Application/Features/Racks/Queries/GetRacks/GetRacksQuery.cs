using MediatR;
using RackingSystem.Application.Features.Racks.DTOs;

namespace RackingSystem.Application.Features.Racks.Queries.GetRacks;

public record GetRacksQuery(int? ZoneId = null) : IRequest<IEnumerable<RackDto>>;
