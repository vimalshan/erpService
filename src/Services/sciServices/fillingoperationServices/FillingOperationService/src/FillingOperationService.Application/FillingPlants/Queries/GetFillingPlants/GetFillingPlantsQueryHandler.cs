using FillingOperationService.Application.DTOs;
using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingPlants.Queries.GetFillingPlants;

public class GetFillingPlantsQueryHandler(IFillingPlantRepository repository)
    : IRequestHandler<GetFillingPlantsQuery, IEnumerable<FillingPlantDto>>
{
    public async Task<IEnumerable<FillingPlantDto>> Handle(GetFillingPlantsQuery request, CancellationToken cancellationToken)
    {
        var plants = request.CompanyUnitId.HasValue
            ? await repository.GetByCompanyUnitAsync(request.CompanyUnitId.Value, cancellationToken)
            : await repository.GetAllAsync(cancellationToken);

        return plants.Select(p => new FillingPlantDto(
            p.FillingPlantId,
            p.CompanyUnitId,
            p.FillingPlantName,
            p.Location,
            p.CreationDate,
            p.SciUserIdCreated));
    }
}
