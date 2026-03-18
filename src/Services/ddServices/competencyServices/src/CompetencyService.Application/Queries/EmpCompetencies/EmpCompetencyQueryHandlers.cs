using MediatR;
using AutoMapper;
using CompetencyService.Application.DTOs;
using CompetencyService.Domain.Interfaces;

namespace CompetencyService.Application.Queries.EmpCompetencies;

public class GetEmpCompetenciesQueryHandler(IEmpSpecificCompetencyRepository repo, IMapper mapper)
    : IRequestHandler<GetEmpCompetenciesQuery, IEnumerable<EmpSpecificCompetencyDto>>
{
    public async Task<IEnumerable<EmpSpecificCompetencyDto>> Handle(
        GetEmpCompetenciesQuery request, CancellationToken cancellationToken)
    {
        var items = await repo.GetByEmpAsync(request.EmpSysId, request.YearId, cancellationToken);
        return items.Select(mapper.Map<EmpSpecificCompetencyDto>);
    }
}
