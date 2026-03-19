using MediatR;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Application.Commands.ProductionPlans;

public record CreateProductionPlanCommand(CreateProductionPlanDto Dto) : IRequest<ProductionPlanDto>;

public record UpdateProductionPlanCommand(UpdateProductionPlanDto Dto) : IRequest<ProductionPlanDto>;

public record CloseProductionPlanCommand(int ProductionPlantId, int SciItemId, int ModifiedBy) : IRequest<ProductionPlanDto>;

public record DeleteProductionPlanCommand(int ProductionPlantId, int SciItemId) : IRequest<bool>;
