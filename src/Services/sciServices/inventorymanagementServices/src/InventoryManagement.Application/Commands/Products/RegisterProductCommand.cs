using InventoryManagement.Application.DTOs;
using MediatR;

namespace InventoryManagement.Application.Commands.Products;

public record RegisterProductCommand(
    string ProductName,
    string? ProductDescription,
    int UnitId,
    int ProductTypeId,
    int CompanyUnitId,
    int CreatedBy) : IRequest<ProductDto>;
