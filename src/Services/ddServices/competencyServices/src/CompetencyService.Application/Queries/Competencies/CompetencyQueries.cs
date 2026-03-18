using MediatR;
using CompetencyService.Application.DTOs;

namespace CompetencyService.Application.Queries.Competencies;

public record GetAllCompetenciesQuery(int Page = 1, int PageSize = 20)
    : IRequest<PagedResult<CompetencyDto>>;

public record GetCompetencyByIdQuery(decimal Id) : IRequest<CompetencyDto?>;

public record GetCompetenciesByTypeQuery(string Type) : IRequest<IEnumerable<CompetencyDto>>;
