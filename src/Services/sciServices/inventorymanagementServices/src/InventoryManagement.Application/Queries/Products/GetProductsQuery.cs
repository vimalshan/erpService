using InventoryManagement.Application.DTOs;
using MediatR;

namespace InventoryManagement.Application.Queries.Products;

public record GetAllProductsQuery : IRequest<IEnumerable<ProductDto>>;

public record GetProductByIdQuery(int ProductId) : IRequest<ProductDto?>;
