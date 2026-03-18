using LovService.Application.DTOs;
using MediatR;

namespace LovService.Application.Queries.ItemData;

public record GetAllItemDataQuery : IRequest<IEnumerable<ItemDataDto>>;

public record GetItemDataByIdQuery(int Id) : IRequest<ItemDataDto?>;

public record SearchItemDataQuery(string? CatName, string? ItemName) : IRequest<IEnumerable<ItemDataDto>>;
