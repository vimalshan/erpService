using MediatR;
using TeamServices.Application.DTOs;

namespace TeamServices.Application.Queries;

public record GetTeamByIdQuery(long TeamId) : IRequest<TeamDto?>;

public record GetAllTeamsQuery() : IRequest<IReadOnlyList<TeamDto>>;

public record GetTeamEmployeesByTeamIdQuery(long TeamId) : IRequest<IReadOnlyList<TeamEmployeeMapDto>>;

public record GetActiveTeamEmployeesQuery(long TeamId, DateTime AsOfDate) : IRequest<IReadOnlyList<TeamEmployeeMapDto>>;

public record GetTeamUnitMapsByTeamIdQuery(long TeamId) : IRequest<IReadOnlyList<TeamUnitMapDto>>;
