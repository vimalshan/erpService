using MediatR;
using CompetencyService.Application.DTOs;

namespace CompetencyService.Application.Commands.RoleSpecific;

public record AssignRoleCompetencyCommand(
    decimal EmpSysId,
    decimal CompetencyId,
    DateTime? EffFrom,
    DateTime? EffTo,
    decimal? ModifiedBy
) : IRequest<RoleSpecificDto>;

public record ExpireRoleCompetencyCommand(
    decimal EmpSysId,
    decimal CompetencyId,
    DateTime EffTo,
    decimal? ModifiedBy
) : IRequest<bool>;
