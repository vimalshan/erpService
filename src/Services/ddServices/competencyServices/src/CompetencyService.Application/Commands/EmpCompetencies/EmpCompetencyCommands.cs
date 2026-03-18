using MediatR;
using CompetencyService.Application.DTOs;

namespace CompetencyService.Application.Commands.EmpCompetencies;

public record AssignEmpCompetencyCommand(
    decimal EmpSysId,
    decimal CompetencyId,
    char CompetencyType,
    decimal YearId,
    decimal? ModifiedBy
) : IRequest<EmpSpecificCompetencyDto>;

public record RemoveEmpCompetencyCommand(
    decimal EmpSysId,
    decimal CompetencyId,
    decimal YearId
) : IRequest<bool>;
