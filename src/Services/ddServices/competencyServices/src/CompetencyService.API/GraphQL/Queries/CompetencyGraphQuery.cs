using MediatR;
using CompetencyService.Application.DTOs;
using CompetencyService.Application.Queries.Competencies;
using CompetencyService.Application.Queries.EmpCompetencies;
using CompetencyService.Infrastructure.DapperQueries;

namespace CompetencyService.API.GraphQL.Queries;

public class CompetencyQuery
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public async Task<IEnumerable<CompetencyDto>> GetCompetencies(
        [Service] IMediator mediator,
        int page = 1, int pageSize = 20,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetAllCompetenciesQuery(page, pageSize), ct);
        return result.Items;
    }

    public async Task<CompetencyDto?> GetCompetency(
        [Service] IMediator mediator,
        decimal id, CancellationToken ct = default)
        => await mediator.Send(new GetCompetencyByIdQuery(id), ct);

    public async Task<IEnumerable<CompetencyDto>> GetCompetenciesByType(
        [Service] IMediator mediator,
        string type, CancellationToken ct = default)
        => await mediator.Send(new GetCompetenciesByTypeQuery(type), ct);

    public async Task<IEnumerable<EmpSpecificCompetencyDto>> GetEmpCompetencies(
        [Service] IMediator mediator,
        decimal empSysId, decimal yearId,
        CancellationToken ct = default)
        => await mediator.Send(new GetEmpCompetenciesQuery(empSysId, yearId), ct);

    [UseFiltering]
    public async Task<IEnumerable<CompetencyDto>> SearchCompetencies(
        [Service] CompetencyDapperQueries dapper,
        string searchTerm)
        => await dapper.SearchCompetenciesAsync(searchTerm);
}
