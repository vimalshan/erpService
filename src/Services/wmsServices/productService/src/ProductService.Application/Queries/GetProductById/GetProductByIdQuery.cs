using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Queries.GetProductById;

public sealed record GetProductByIdQuery(int ProductId) : IRequest<ProductDto?>;
