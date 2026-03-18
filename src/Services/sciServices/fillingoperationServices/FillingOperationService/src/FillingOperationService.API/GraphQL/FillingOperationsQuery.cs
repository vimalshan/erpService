using FillingOperationService.Application.DTOs;
using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlantById;
using FillingOperationService.Application.FillingPlants.Queries.GetFillingPlants;
using FillingOperationService.Application.FillingLines.Queries.GetFillingLines;
using FillingOperationService.Application.FillingCapacities.Queries.GetFillingCapacity;
using MediatR;

namespace FillingOperationService.API.GraphQL;

public class FillingOperationsQuery
{
    public async Task<IEnumerable<FillingPlantDto>> GetFillingPlants(
        [Service] IMediator mediator,
        int? companyUnitId = null,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetFillingPlantsQuery(companyUnitId), cancellationToken);

    public async Task<FillingPlantDto?> GetFillingPlantById(
        [Service] IMediator mediator,
        int id,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetFillingPlantByIdQuery(id), cancellationToken);

    public async Task<IEnumerable<FillingLineDto>> GetFillingLines(
        [Service] IMediator mediator,
        int plantId,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetFillingLinesQuery(plantId), cancellationToken);

    public async Task<IEnumerable<FillingCapacityDto>> GetFillingCapacity(
        [Service] IMediator mediator,
        int groupId,
        int? productId = null,
        CancellationToken cancellationToken = default)
        => await mediator.Send(new GetFillingCapacityQuery(groupId, productId), cancellationToken);
}
