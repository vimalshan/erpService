using FillingOperationService.Application.FillingPlants.Commands.CreateFillingPlant;
using FillingOperationService.Application.FillingLines.Commands.CreateFillingLine;
using MediatR;

namespace FillingOperationService.API.GraphQL;

public class FillingOperationsMutation
{
    public async Task<int> CreateFillingPlant(
        [Service] IMediator mediator,
        int companyUnitId,
        string plantName,
        string location,
        int createdBy,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new CreateFillingPlantCommand(companyUnitId, plantName, location, createdBy), cancellationToken);

    public async Task<int> CreateFillingLine(
        [Service] IMediator mediator,
        int fillingPlantId,
        string fillingLineName,
        int noOfFillingPoints,
        int? packageTypeId,
        int createdBy,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new CreateFillingLineCommand(fillingPlantId, fillingLineName, noOfFillingPoints, packageTypeId, createdBy), cancellationToken);
}
