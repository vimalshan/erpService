using MediatR;
using TeamServices.Application.DTOs;

namespace TeamServices.Application.Commands;

public record CreateTeamCommand(long TeamId, string TeamName, long ModifiedBy) : IRequest<TeamDto>;

public record UpdateTeamCommand(long TeamId, string TeamName, long ModifiedBy) : IRequest<TeamDto>;

public record DeleteTeamCommand(long TeamId) : IRequest<Unit>;

// Employee Map Commands
public record AddTeamEmployeeCommand(long Id, long TeamId, long EmployeeSysId, DateTime EffectiveDate, DateTime? CloseDate, long ModifiedBy) : IRequest<TeamEmployeeMapDto>;

public record UpdateTeamEmployeeCommand(long Id, long TeamId, long EmployeeSysId, DateTime EffectiveDate, DateTime? CloseDate, long ModifiedBy) : IRequest<TeamEmployeeMapDto>;

public record DeleteTeamEmployeeCommand(long Id) : IRequest<Unit>;

// Unit Map Commands
public record AddTeamUnitMapCommand(long MapId, long TeamId, long UnitId, string GradeCategory, long? CadreId, long ModifiedBy) : IRequest<TeamUnitMapDto>;

public record UpdateTeamUnitMapCommand(long MapId, long TeamId, long UnitId, string GradeCategory, long? CadreId, long ModifiedBy) : IRequest<TeamUnitMapDto>;

public record DeleteTeamUnitMapCommand(long MapId) : IRequest<Unit>;
