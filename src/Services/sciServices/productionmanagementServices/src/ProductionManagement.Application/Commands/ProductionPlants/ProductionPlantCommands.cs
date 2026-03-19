using MediatR;
using ProductionManagement.Application.DTOs;

namespace ProductionManagement.Application.Commands.ProductionPlants;

// Create
public record CreateProductionPlantCommand(CreateProductionPlantDto Dto) : IRequest<ProductionPlantDto>;

// Update
public record UpdateProductionPlantCommand(UpdateProductionPlantDto Dto) : IRequest<ProductionPlantDto>;

// Delete
public record DeleteProductionPlantCommand(int ProductionPlantId) : IRequest<bool>;

// Map Product to Plant
public record MapProductToPlantCommand(CreateProductionPlantProductMapDto Dto) : IRequest<ProductionPlantProductMapDto>;
