using FillingOperationService.Domain.Entities;
using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingPlants.Commands.CreateFillingPlant;

public class CreateFillingPlantCommandHandler(IFillingPlantRepository repository)
    : IRequestHandler<CreateFillingPlantCommand, int>
{
    public async Task<int> Handle(CreateFillingPlantCommand request, CancellationToken cancellationToken)
    {
        var plant = FillingPlant.Create(request.CompanyUnitId, request.PlantName, request.Location, request.CreatedBy);
        await repository.AddAsync(plant, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);
        return plant.FillingPlantId;
    }
}
