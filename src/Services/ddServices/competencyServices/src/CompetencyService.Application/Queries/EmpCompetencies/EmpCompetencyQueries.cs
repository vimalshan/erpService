using MediatR;
using CompetencyService.Application.DTOs;

namespace CompetencyService.Application.Queries.EmpCompetencies;

public record GetEmpCompetenciesQuery(decimal EmpSysId, decimal YearId)
    : IRequest<IEnumerable<EmpSpecificCompetencyDto>>;
