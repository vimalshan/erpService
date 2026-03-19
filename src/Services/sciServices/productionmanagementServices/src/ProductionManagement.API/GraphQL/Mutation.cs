using MediatR;
using ProductionManagement.Application.Commands.ProductionPlants;
using ProductionManagement.Application.Commands.ProductionPlans;
using ProductionManagement.Application.Commands.Norms;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.API.GraphQL;

public class Mutation
{
    public async Task<ProductionPlantDto> CreateProductionPlant(
        [Service] IMediator mediator,
        CreateProductionPlantDto input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CreateProductionPlantCommand(input), cancellationToken);
    }

    public async Task<ProductionPlantDto> UpdateProductionPlant(
        [Service] IMediator mediator,
        UpdateProductionPlantDto input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new UpdateProductionPlantCommand(input), cancellationToken);
    }

    public async Task<bool> DeleteProductionPlant(
        [Service] IMediator mediator,
        int productionPlantId,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new DeleteProductionPlantCommand(productionPlantId), cancellationToken);
    }

    public async Task<ProductionPlanDto> CreateProductionPlan(
        [Service] IMediator mediator,
        CreateProductionPlanDto input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CreateProductionPlanCommand(input), cancellationToken);
    }

    public async Task<ProductionPlanDto> CloseProductionPlan(
        [Service] IMediator mediator,
        int productionPlantId,
        int sciItemId,
        int modifiedBy,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CloseProductionPlanCommand(productionPlantId, sciItemId, modifiedBy), cancellationToken);
    }

    public async Task<NormsMainDto> CreateNorm(
        [Service] IMediator mediator,
        CreateNormsMainDto input,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CreateNormsMainCommand(input), cancellationToken);
    }

    public async Task<NormsMainDto> CloseNorm(
        [Service] IMediator mediator,
        long normNo,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(new CloseNormsMainCommand(normNo), cancellationToken);
    }
}
