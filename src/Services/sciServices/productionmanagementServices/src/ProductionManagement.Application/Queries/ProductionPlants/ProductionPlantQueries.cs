using MediatR;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Application.Queries.ProductionPlants;

public record GetAllProductionPlantsQuery : IRequest<IReadOnlyList<ProductionPlantDto>>;
public record GetProductionPlantByIdQuery(int ProductionPlantId) : IRequest<ProductionPlantDto?>;
