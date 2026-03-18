using FillingOperationService.Application.DTOs;
using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingPlants.Queries.GetFillingPlantById;

public class GetFillingPlantByIdQueryHandler(IFillingPlantRepository repository)
    : IRequestHandler<GetFillingPlantByIdQuery, FillingPlantDto?>
{
    public async Task<FillingPlantDto?> Handle(GetFillingPlantByIdQuery request, CancellationToken cancellationToken)
    {
        var plant = await repository.GetByIdAsync(request.FillingPlantId, cancellationToken);
        if (plant is null) return null;

        return new FillingPlantDto(
            plant.FillingPlantId,
            plant.CompanyUnitId,
            plant.FillingPlantName,
            plant.Location,
            plant.CreationDate,
            plant.SciUserIdCreated);
    }
}
