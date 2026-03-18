using FillingOperationService.Domain.Interfaces;
using MediatR;

namespace FillingOperationService.Application.FillingPlants.Commands.UpdateFillingPlant;

public class UpdateFillingPlantCommandHandler(IFillingPlantRepository repository)
    : IRequestHandler<UpdateFillingPlantCommand>
{
    public async Task Handle(UpdateFillingPlantCommand request, CancellationToken cancellationToken)
    {
        var plant = await repository.GetByIdAsync(request.FillingPlantId, cancellationToken)
            ?? throw new KeyNotFoundException($"Filling plant {request.FillingPlantId} not found.");

        plant.Update(request.PlantName, request.Location, request.ModifiedBy);
        repository.Update(plant);
        await repository.SaveChangesAsync(cancellationToken);
    }
}
