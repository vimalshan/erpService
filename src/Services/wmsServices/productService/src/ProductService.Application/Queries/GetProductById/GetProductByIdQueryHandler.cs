using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler(IProductRepository repo, IMapper mapper)
    : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var product = await repo.GetByIdAsync(request.ProductId, ct);
        return product is null ? null : mapper.Map<ProductDto>(product);
    }
}
