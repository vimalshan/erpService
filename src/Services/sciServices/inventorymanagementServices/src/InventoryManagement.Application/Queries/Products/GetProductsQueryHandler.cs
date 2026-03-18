using AutoMapper;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Queries.Products;

public sealed class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery request, CancellationToken ct)
        => _mapper.Map<IEnumerable<ProductDto>>(await _repo.GetAllAsync(ct));
}

public sealed class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto?>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(IProductRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<ProductDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.ProductId, ct);
        return entity is null ? null : _mapper.Map<ProductDto>(entity);
    }
}
