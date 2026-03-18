using MediatR;

namespace FillingOperationService.Application.FillingPlants.Commands.CreateFillingPlant;

public record CreateFillingPlantCommand(
    int CompanyUnitId,
    string PlantName,
    string Location,
    int CreatedBy
) : IRequest<int>;
