using MediatR;

namespace InventoryManagement.Application.Commands.Products;

public record UpdateProductCommand(
    int ProductId,
    string ProductName,
    string? ProductDescription,
    int UnitId,
    int ModifiedBy) : IRequest;
