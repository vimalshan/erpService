using MediatR;
using ProductService.Application.DTOs;

namespace ProductService.Application.Commands.UpdateProduct;

public sealed record UpdateProductCommand(int ProductId, UpdateProductDto Dto) : IRequest<ProductDto>;
