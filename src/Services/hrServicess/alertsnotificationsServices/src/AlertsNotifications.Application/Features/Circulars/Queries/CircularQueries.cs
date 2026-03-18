using AlertsNotifications.Application.DTOs;
using MediatR;

namespace AlertsNotifications.Application.Features.Circulars.Queries;

public record GetAllCircularsQuery : IRequest<IEnumerable<CircularDto>>;

public record GetCircularByIdQuery(long CircularId) : IRequest<CircularDto?>;

public record GetCircularsByStatusQuery(char Status) : IRequest<IEnumerable<CircularDto>>;

public record GetCircularsByOrgIdQuery(long OrgId) : IRequest<IEnumerable<CircularDto>>;
