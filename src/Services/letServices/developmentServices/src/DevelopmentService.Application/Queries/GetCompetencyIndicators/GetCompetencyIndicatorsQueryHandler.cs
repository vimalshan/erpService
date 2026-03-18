using MediatR;
using DevelopmentService.Application.DTOs;
using DevelopmentService.Application.Mappings;
using DevelopmentService.Domain.Interfaces;

namespace DevelopmentService.Application.Queries.GetCompetencyIndicators;

public class GetCompetencyIndicatorsQueryHandler
    : IRequestHandler<GetCompetencyIndicatorsQuery, IEnumerable<CompetencyIndDto>>
{
    private readonly ICompetencyRepository _repository;

    public GetCompetencyIndicatorsQueryHandler(ICompetencyRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<CompetencyIndDto>> Handle(
        GetCompetencyIndicatorsQuery request, CancellationToken cancellationToken)
    {
        var indicators = await _repository.GetIndicatorsAsync(request.CompNum, request.Band, cancellationToken);
        return indicators.ToDtos();
    }
}
