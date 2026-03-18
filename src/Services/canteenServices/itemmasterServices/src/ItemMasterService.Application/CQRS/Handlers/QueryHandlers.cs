using MediatR;
using ItemMasterService.Application.CQRS.Handlers;
using ItemMasterService.Application.CQRS.Queries;
using ItemMasterService.Application.DTOs;
using ItemMasterService.Domain.Interfaces;

namespace ItemMasterService.Application.CQRS.Handlers;

public class GetCanteenItemByIdQueryHandler : IRequestHandler<GetCanteenItemByIdQuery, CanteenItemMasterDto?>
{
    private readonly ICanteenItemRepository _repo;

    public GetCanteenItemByIdQueryHandler(ICanteenItemRepository repo) => _repo = repo;

    public async Task<CanteenItemMasterDto?> Handle(GetCanteenItemByIdQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByIdAsync(request.CanteenUnitCode, request.ItemCode, ct);
        return entity is null ? null : CreateCanteenItemCommandHandler.MapToDto(entity);
    }
}

public class GetAllCanteenItemsQueryHandler : IRequestHandler<GetAllCanteenItemsQuery, IEnumerable<CanteenItemMasterDto>>
{
    private readonly ICanteenItemRepository _repo;

    public GetAllCanteenItemsQueryHandler(ICanteenItemRepository repo) => _repo = repo;

    public async Task<IEnumerable<CanteenItemMasterDto>> Handle(GetAllCanteenItemsQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(request.CanteenUnitCode, ct);
        return items.Select(CreateCanteenItemCommandHandler.MapToDto);
    }
}

public class GetItemPriceQueryHandler : IRequestHandler<GetItemPriceQuery, CanteenItemPriceMasterDto?>
{
    private readonly ICanteenItemPriceRepository _repo;

    public GetItemPriceQueryHandler(ICanteenItemPriceRepository repo) => _repo = repo;

    public async Task<CanteenItemPriceMasterDto?> Handle(GetItemPriceQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetActiveAsync(request.CanteenUnitCode, request.ItemCode, ct);
        return entity is null ? null : CreateItemPriceCommandHandler.MapPriceDto(entity);
    }
}

public class GetItemPriceHistoryQueryHandler : IRequestHandler<GetItemPriceHistoryQuery, IEnumerable<CanteenItemPriceMasterDto>>
{
    private readonly ICanteenItemPriceRepository _repo;

    public GetItemPriceHistoryQueryHandler(ICanteenItemPriceRepository repo) => _repo = repo;

    public async Task<IEnumerable<CanteenItemPriceMasterDto>> Handle(GetItemPriceHistoryQuery request, CancellationToken ct)
    {
        var items = await _repo.GetHistoryAsync(request.CanteenUnitCode, request.ItemCode, ct);
        return items.Select(CreateItemPriceCommandHandler.MapPriceDto);
    }
}

public class GetGradeItemPriceQueryHandler : IRequestHandler<GetGradeItemPriceQuery, CanteenGradeItemPriceDto?>
{
    private readonly ICanteenGradeItemPriceRepository _repo;

    public GetGradeItemPriceQueryHandler(ICanteenGradeItemPriceRepository repo) => _repo = repo;

    public async Task<CanteenGradeItemPriceDto?> Handle(GetGradeItemPriceQuery request, CancellationToken ct)
    {
        var entity = await _repo.GetByUnitCodeAsync(request.CanteenUnitCode, ct);
        return entity is null ? null : CreateGradeItemPriceCommandHandler.MapGradeDto(entity);
    }
}

public class GetAllGradeItemPricesQueryHandler : IRequestHandler<GetAllGradeItemPricesQuery, IEnumerable<CanteenGradeItemPriceDto>>
{
    private readonly ICanteenGradeItemPriceRepository _repo;

    public GetAllGradeItemPricesQueryHandler(ICanteenGradeItemPriceRepository repo) => _repo = repo;

    public async Task<IEnumerable<CanteenGradeItemPriceDto>> Handle(GetAllGradeItemPricesQuery request, CancellationToken ct)
    {
        var items = await _repo.GetAllAsync(ct);
        return items.Select(CreateGradeItemPriceCommandHandler.MapGradeDto);
    }
}
