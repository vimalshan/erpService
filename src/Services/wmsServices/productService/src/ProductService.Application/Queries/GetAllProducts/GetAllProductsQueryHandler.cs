using AutoMapper;
using MediatR;
using ProductService.Application.DTOs;
using ProductService.Domain.Interfaces;

namespace ProductService.Application.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler(IProductRepository repo, IMapper mapper)
    : IRequestHandler<GetAllProductsQuery, IReadOnlyList<ProductDto>>
{
    public async Task<IReadOnlyList<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
    {
        var products = await repo.GetAllAsync(ct);
        return mapper.Map<IReadOnlyList<ProductDto>>(products);
    }
}
