using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.CreateProduct;

public sealed record CreateProductCommand(CreateProductDto Dto) : IRequest<ProductDto>;
