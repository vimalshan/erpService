using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.GetAllProducts;

public sealed record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>;
