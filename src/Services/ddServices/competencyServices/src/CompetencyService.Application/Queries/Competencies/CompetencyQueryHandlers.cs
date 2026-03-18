using MediatR;
using AutoMapper;
using CompetencyService.Application.DTOs;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Application.Queries.Competencies;

public class GetAllCompetenciesQueryHandler(ICompetencyRepository repo, IMapper mapper)
    : IRequestHandler<GetAllCompetenciesQuery, PagedResult<CompetencyDto>>
{
    public async Task<PagedResult<CompetencyDto>> Handle(
        GetAllCompetenciesQuery request, CancellationToken cancellationToken)
    {
        var all = (await repo.GetAllAsync(cancellationToken)).ToList();
        var paged = all
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(mapper.Map<CompetencyDto>);
        return new PagedResult<CompetencyDto>(paged, all.Count, request.Page, request.PageSize);
    }
}

public class GetCompetencyByIdQueryHandler(ICompetencyRepository repo, IMapper mapper)
    : IRequestHandler<GetCompetencyByIdQuery, CompetencyDto?>
{
    public async Task<CompetencyDto?> Handle(
        GetCompetencyByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await repo.GetByIdAsync(request.Id, cancellationToken);
        return entity is null ? null : mapper.Map<CompetencyDto>(entity);
    }
}

public class GetCompetenciesByTypeQueryHandler(ICompetencyRepository repo, IMapper mapper)
    : IRequestHandler<GetCompetenciesByTypeQuery, IEnumerable<CompetencyDto>>
{
    public async Task<IEnumerable<CompetencyDto>> Handle(
        GetCompetenciesByTypeQuery request, CancellationToken cancellationToken)
    {
        var items = await repo.GetByTypeAsync(request.Type, cancellationToken);
        return items.Select(mapper.Map<CompetencyDto>);
    }
}
