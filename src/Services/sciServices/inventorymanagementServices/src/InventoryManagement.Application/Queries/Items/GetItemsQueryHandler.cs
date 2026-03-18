using AutoMapper;
using InventoryManagement.Application.DTOs;
using InventoryManagement.Domain.Interfaces;
using MediatR;

namespace InventoryManagement.Application.Queries.Items;

public sealed class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, IEnumerable<ItemDto>>
{
    private readonly IItemRepository _repo;
    private readonly IMapper _mapper;

    public GetAllItemsQueryHandler(IItemRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<ItemDto>> Handle(GetAllItemsQuery request, CancellationToken ct)
        => _mapper.Map<IEnumerable<ItemDto>>(await _repo.GetAllAsync(ct));
}

public sealed class GetItemByIdQueryHandler : IRequestHandler<GetItemByIdQuery, ItemDto?>
{
    private readonly IItemRepository _repo;
    private readonly IMapper _mapper;

    public GetItemByIdQueryHandler(IItemRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<ItemDto?> Handle(GetItemByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.SciItemId, ct);
        return entity is null ? null : _mapper.Map<ItemDto>(entity);
    }
}

public sealed class GetItemsByProductQueryHandler : IRequestHandler<GetItemsByProductQuery, IEnumerable<ItemDto>>
{
    private readonly IItemRepository _repo;
    private readonly IMapper _mapper;

    public GetItemsByProductQueryHandler(IItemRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<IEnumerable<ItemDto>> Handle(GetItemsByProductQuery request, CancellationToken ct)
        => _mapper.Map<IEnumerable<ItemDto>>(await _repo.GetByProductIdAsync(request.ProductId, ct));
}

public sealed class GetItemByOracleCodeQueryHandler : IRequestHandler<GetItemByOracleCodeQuery, ItemDto?>
{
    private readonly IItemRepository _repo;
    private readonly IMapper _mapper;

    public GetItemByOracleCodeQueryHandler(IItemRepository repo, IMapper mapper) { _repo = repo; _mapper = mapper; }

    public async Task<ItemDto?> Handle(GetItemByOracleCodeQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByOracleCodeAsync(request.OracleCode, ct);
        return entity is null ? null : _mapper.Map<ItemDto>(entity);
    }
}
