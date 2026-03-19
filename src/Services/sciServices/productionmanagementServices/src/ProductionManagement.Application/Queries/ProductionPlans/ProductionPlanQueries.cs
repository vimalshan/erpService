using MediatR;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Application.Queries.ProductionPlans;

public record GetAllProductionPlansQuery : IRequest<IReadOnlyList<ProductionPlanDto>>;
public record GetProductionPlansByPlantIdQuery(int ProductionPlantId) : IRequest<IReadOnlyList<ProductionPlanDto>>;
public record GetProductionPlanByIdQuery(int ProductionPlantId, int SciItemId) : IRequest<ProductionPlanDto?>;
