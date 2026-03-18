using LovService.Application.DTOs;
using LovService.Application.Interfaces;
using MediatR;

namespace LovService.Application.Queries.ItemData;

public class GetAllItemDataQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetAllItemDataQuery, IEnumerable<ItemDataDto>>
{
    public async Task<IEnumerable<ItemDataDto>> Handle(GetAllItemDataQuery request, CancellationToken ct)
    {
        var items = await uow.ItemData.GetAllAsync(ct);
        return items.Select(i => new ItemDataDto(i.Id, i.CatName, i.ItemName, i.Make, i.Uom, i.Price));
    }
}

public class GetItemDataByIdQueryHandler(IUnitOfWork uow)
    : IRequestHandler<GetItemDataByIdQuery, ItemDataDto?>
{
    public async Task<ItemDataDto?> Handle(GetItemDataByIdQuery request, CancellationToken ct)
    {
        var item = await uow.ItemData.GetByIdAsync(request.Id, ct);
        return item is null ? null : new ItemDataDto(item.Id, item.CatName, item.ItemName, item.Make, item.Uom, item.Price);
    }
}

public class SearchItemDataQueryHandler(IUnitOfWork uow)
    : IRequestHandler<SearchItemDataQuery, IEnumerable<ItemDataDto>>
{
    public async Task<IEnumerable<ItemDataDto>> Handle(SearchItemDataQuery request, CancellationToken ct)
    {
        var items = await uow.ItemData.SearchAsync(request.CatName, request.ItemName, ct);
        return items.Select(i => new ItemDataDto(i.Id, i.CatName, i.ItemName, i.Make, i.Uom, i.Price));
    }
}
