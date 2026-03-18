using MediatR;

namespace InventoryManagement.Application.Commands.Products;

public record DeleteProductCommand(int ProductId) : IRequest;
