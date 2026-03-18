using MediatR;

namespace FillingOperationService.Application.FillingPlants.Commands.UpdateFillingPlant;

public record UpdateFillingPlantCommand(
    int FillingPlantId,
    string PlantName,
    string Location,
    int ModifiedBy
) : IRequest;
